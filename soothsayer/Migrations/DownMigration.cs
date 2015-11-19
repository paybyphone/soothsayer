using System.Collections.Generic;
using System.Linq;
using soothsayer.Infrastructure;
using soothsayer.Infrastructure.IO;
using soothsayer.Oracle;
using soothsayer.Scripts;

namespace soothsayer.Migrations
{
    public class DownMigration : IMigration
    {
        private readonly IDatabaseMetadataProvider _databaseMetadataProvider;
        private readonly IVersionRespository _versionRespository;
        private readonly IAppliedScriptsRepository _appliedScriptsRepository;
        private readonly bool _force;

        public DownMigration(IDatabaseMetadataProvider databaseMetadataProvider, IVersionRespository versionRespository, IAppliedScriptsRepository appliedScriptsRepository, bool force)
        {
            _databaseMetadataProvider = databaseMetadataProvider;
            _versionRespository = versionRespository;
            _appliedScriptsRepository = appliedScriptsRepository;
            _force = force;
        }

        public void Migrate(IEnumerable<IManoeuvre> migrationManoeuvres, DatabaseVersion currentVersion, long? targetVersionNumber, IScriptRunner scriptRunner, string schema, string tablespace)
        {
            if (_databaseMetadataProvider.SchemaExists(schema))
            {
                if (_versionRespository.GetCurrentVersion(schema).IsNotNull())
                {
                    DowngradeDatabase(migrationManoeuvres.Select(m => m.BackwardScript), currentVersion, targetVersionNumber, scriptRunner, schema);
                }
                else
                {
                    Output.Warn("Database schema '{0}' exists, but there are no recorded database versions. Database was most likely not terminated.".FormatWith(schema));
                }
            }
        }

        private void DowngradeDatabase(IEnumerable<IScript> migrationScripts, DatabaseVersion currentVersion, long? targetVersionNumber, IScriptRunner scriptRunner, string schema)
        {
            var applicableScripts = migrationScripts.Where(s => currentVersion.IsNull() || s.Version <= currentVersion.Version)
                .Where(s => !targetVersionNumber.HasValue || s.Version > targetVersionNumber).ToArray();

            if (applicableScripts.IsNullOrEmpty())
            {
                Output.Warn("No migration scripts need to be run. Nothing will be done.");
            }
            else
            {
                Output.Info("The following scripts will be applied to the database:");
                foreach (var script in applicableScripts)
                {
                    Output.Info(script.Name, 1);
                }

                Output.EmptyLine();
                Prompt.ForAnyKey("Press any key to start the 'down' migration. Ctrl-C to abort.");

                foreach (var script in applicableScripts)
                {
                    Output.Info("Executing script: {0}".FormatWith(script.Path));

                    try
                    {
                        scriptRunner.Execute(script);
                    }
                    catch (SqlPlusException)
                    {
                        if (!_force)
                        {
                            throw;
                        }
                    }

                    Output.Info("Script '{0}' completed.".FormatWith(script.Name));

                    Output.Text("Removing script contents for script '{0}' from applied scripts table".FormatWith(script.Name));
                    _appliedScriptsRepository.RemoveAppliedScript(script.Version, schema);

                    Output.Text("Removing version {0} for script '{1}' from version table".FormatWith(script.Version, script.Name));
                    _versionRespository.RemoveVersion(script.AsDatabaseVersion(), schema);
                }
            }
        }
    }
}
