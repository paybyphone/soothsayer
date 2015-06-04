using System.Collections.Generic;
using System.Linq;
using soothsayer.Infrastructure;
using soothsayer.Infrastructure.IO;
using soothsayer.Oracle;
using soothsayer.Scripts;

namespace soothsayer.Migrations
{
    public class TermMigration : IMigration
    {
        private readonly IDatabaseMetadataProvider _databaseMetadataProvider;
        private readonly bool _force;

        public TermMigration(IDatabaseMetadataProvider databaseMetadataProvider, bool force)
        {
            _databaseMetadataProvider = databaseMetadataProvider;
            _force = force;
        }

        public void Migrate(IEnumerable<IScript> migrationScripts, DatabaseVersion currentVersion, long? targetVersion, IScriptRunner scriptRunner, string schema, string tablespace)
        {
            if (_databaseMetadataProvider.SchemaExists(schema))
            {
                TerminateDatabase(migrationScripts, scriptRunner, schema);
            }
        }

        private void TerminateDatabase(IEnumerable<IScript> termScripts, IScriptRunner scriptRunner, string schema)
        {
            var termScriptsList = termScripts as IList<IScript> ?? termScripts.ToList();
            if (termScriptsList.IsNullOrEmpty())
            {
                Output.Warn("No termination scripts found to destroy the database. Nothing will be done.");
            }
            else
            {
                Output.Info("The following scripts will be used to terminate the tablespace and schema:");
                foreach (var script in termScriptsList)
                {
                    Output.Info(script.Name, 1);
                }

                Output.EmptyLine();
                Prompt.ForAnyKey("Press any key to start the 'term' migration. Ctrl-C to abort.");

                foreach (var script in termScriptsList)
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
                }

                Output.Info("Termination scripts completed.'".FormatWith(schema));
            }

            Output.EmptyLine();
        }
    }
}
