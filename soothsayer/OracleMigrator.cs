using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using soothsayer.Infrastructure;
using soothsayer.Migrations;
using soothsayer.Oracle;
using soothsayer.Scanners;
using soothsayer.Scripts;

namespace soothsayer
{
    public class OracleMigrator : IMigrator
    {
        private readonly IConnectionFactory _connectionFactory;
        private readonly IScriptScannerFactory _scriptScannerFactory;

        public OracleMigrator(IConnectionFactory connectionFactory, IScriptScannerFactory scriptScannerFactory)
        {
            _connectionFactory = connectionFactory;
            _scriptScannerFactory = scriptScannerFactory;
        }

        public void Migrate(DatabaseConnectionInfo databaseConnectionInfo, MigrationInfo migrationInfo)
        {
            using (var connection = _connectionFactory.Create(databaseConnectionInfo))
            {
                Output.Text("Connected to oracle database on connection string '{0}'.".FormatWith(databaseConnectionInfo.ConnectionString));
                Output.EmptyLine();

                Output.Text("Checking for the current database version.");
                var oracleMetadataProvider = new OracleMetadataProvider(connection);
                var oracleVersioning = new OracleVersionRespository(connection);
                var currentVersion = oracleMetadataProvider.SchemaExists(migrationInfo.TargetSchema) ? oracleVersioning.GetCurrentVersion(migrationInfo.TargetSchema) : null;
                Output.Info("The current database version is: {0}".FormatWith(currentVersion.IsNotNull() ? currentVersion.Version.ToString(CultureInfo.InvariantCulture) : "<empty>"));
                Output.EmptyLine();

                Output.Info("Scanning input folder '{0}' for scripts...".FormatWith(migrationInfo.ScriptFolder));

                var initScripts = ScanForScripts(migrationInfo, ScriptFolders.Init, _scriptScannerFactory.Create(ScriptFolders.Init)).ToArray();
                var upScripts = ScanForScripts(migrationInfo, ScriptFolders.Up, _scriptScannerFactory.Create(ScriptFolders.Up)).ToArray();
                var downScripts = ScanForScripts(migrationInfo, ScriptFolders.Down, _scriptScannerFactory.Create(ScriptFolders.Down)).ToArray();
                var termScripts = ScanForScripts(migrationInfo, ScriptFolders.Term, _scriptScannerFactory.Create(ScriptFolders.Term)).ToArray();
                Output.EmptyLine();

                if (migrationInfo.TargetVersion.HasValue)
                {
                    Output.Info("Target database version was provided, will target migrating the database to version {0}".FormatWith(migrationInfo.TargetVersion.Value));
                }

                VerifyDownScripts(upScripts, downScripts);

                var scriptRunner = new SqlPlusScriptRunner(new ProcessRunner(), databaseConnectionInfo);
                RunMigration(migrationInfo, currentVersion, initScripts, upScripts, downScripts, termScripts, scriptRunner, oracleMetadataProvider, oracleVersioning);

                if (oracleMetadataProvider.SchemaExists(migrationInfo.TargetSchema))
                {
                    var newVersion = oracleVersioning.GetCurrentVersion(migrationInfo.TargetSchema);
                    Output.Info("Database version is now: {0}".FormatWith(newVersion.IsNotNull() ? newVersion.Version.ToString(CultureInfo.InvariantCulture) : "<empty>"));
                }
                else
                {
                    Output.Info("Target schema '{0}' no longer exists.".FormatWith(migrationInfo.TargetSchema));
                }
            }
        }

        private static IEnumerable<Script> ScanForScripts(MigrationInfo migrationInfo, string migrationFolder, IScriptScanner scanner)
        {
            var scripts = scanner.Scan(migrationInfo.ScriptFolder.Whack(migrationFolder), migrationInfo.TargetEnvironment).ToArray();

            Output.Text("Found the following '{0}' scripts:".FormatWith(migrationFolder));

            foreach (var script in scripts)
            {
                Output.Text(script.Name, 1);
            }

            return scripts;
        }

        private static void VerifyDownScripts(IEnumerable<Script> upScripts, IEnumerable<Script> downScripts)
        {
            var withoutRollback = upScripts.Where(u => downScripts.All(d => d.Version != u.Version)).ToArray();

            if (withoutRollback.Any())
            {
                Output.Warn("The following 'up' scripts do not have a corresponding 'down' script, any rollback may not work as expected:");

                foreach (var script in withoutRollback)
                {
                    Output.Warn(script.Name, 1);
                }

                Output.EmptyLine();
            }
        }

        private static void RunMigration(MigrationInfo migrationInfo, DatabaseVersion currentVersion, IEnumerable<Script> initScripts, IEnumerable<Script> upScripts,
            IEnumerable<Script> downScripts, IEnumerable<Script> termScripts, SqlPlusScriptRunner scriptRunner, OracleMetadataProvider databaseMetadataProvider, OracleVersionRespository versionRespository)
        {
            if (migrationInfo.Direction == MigrationDirection.Down)
            {
                var downMigration = new DownMigration(databaseMetadataProvider, versionRespository, migrationInfo.Forced);
                downMigration.Migrate(downScripts, currentVersion, migrationInfo.TargetVersion, scriptRunner, migrationInfo.TargetSchema, migrationInfo.TargetTablespace);

                if (!migrationInfo.TargetVersion.HasValue)
                {
                    var termMigration = new TermMigration(databaseMetadataProvider, migrationInfo.Forced);
                    termMigration.Migrate(termScripts, currentVersion, migrationInfo.TargetVersion, scriptRunner, migrationInfo.TargetSchema, migrationInfo.TargetTablespace);
                }
                else
                {
                    Output.Info("A target version was provided, termination scripts will not be executed.");
                }
            }
            else
            {
                var initMigration = new InitMigration(databaseMetadataProvider, versionRespository, migrationInfo.Forced);
                initMigration.Migrate(initScripts, currentVersion, migrationInfo.TargetVersion, scriptRunner, migrationInfo.TargetSchema, migrationInfo.TargetTablespace);

                var upMigration = new UpMigration(versionRespository, migrationInfo.Forced);
                upMigration.Migrate(upScripts, currentVersion, migrationInfo.TargetVersion, scriptRunner, migrationInfo.TargetSchema, migrationInfo.TargetTablespace);
            }
        }
    }
}
