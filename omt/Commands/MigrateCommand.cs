using System.Reflection;
using CommandLine;
using omt.Infrastructure;

namespace omt.Commands
{
    public class MigrateCommand : DatabaseCommand<MigrateCommandOptions>
    {
        private readonly IMigrator _migrator;

        public MigrateCommand(ISecureConsole secureConsole, IMigrator migrator) : base(secureConsole)
        {
            _migrator = migrator;
        }

        public override string CommandText
        {
            get { return "migrate"; }
        }

        protected override void ExecuteCore(string[] arguments)
        {
            var migrateCommandOptions = new MigrateCommandOptions();

            if (Parser.Default.ParseArguments(arguments, migrateCommandOptions))
            {
                Output.Info("omt {0}-beta".FormatWith(Assembly.GetExecutingAssembly().GetName().Version));
                Output.EmptyLine();

                Prompt.NoPrompts = migrateCommandOptions.NoPrompt;
                if (migrateCommandOptions.NoPrompt)
                {
                    Output.Info("Running in noprompt mode, omt will run migrations without prompting for any confirmations.");
                }

                Output.Text("Migration scripts should be located in '{0}'.".FormatWith(migrateCommandOptions.InputFolder));
                Output.Info("The target environment is: {0}".FormatWith(migrateCommandOptions.Environment));
                Output.Text("Scripts will be run against the following schema '{0}' using username '{1}'.".FormatWith(migrateCommandOptions.Schema, migrateCommandOptions.Username));

                Output.Text(migrateCommandOptions.Version.HasValue
                    ? "Target database version specified, will aim to bring the database up to version {0}.".FormatWith(migrateCommandOptions.Version.Value)
                    : "No target database version specified, will aim to bring the database up to the latest version.");

                string password = GetOraclePassword(migrateCommandOptions);

                var databaseConnectionInfo = new DatabaseConnectionInfo(migrateCommandOptions.ConnectionString, migrateCommandOptions.Username, password);
                var migrationInfo = new MigrationInfo(migrateCommandOptions.Down ? MigrationDirection.Down : MigrationDirection.Up,
                    migrateCommandOptions.InputFolder,
                    migrateCommandOptions.Schema,
                    migrateCommandOptions.Tablespace,
                    migrateCommandOptions.Environment,
                    migrateCommandOptions.Version,
                    migrateCommandOptions.Force);

                _migrator.Migrate(databaseConnectionInfo, migrationInfo);

                Output.EmptyLine();
                Prompt.ForAnyKey("Database migration completed. Press any key to exit.");
            }
        }
    }
}