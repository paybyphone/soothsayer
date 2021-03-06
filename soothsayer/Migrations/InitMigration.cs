﻿using System.Collections.Generic;
using System.Linq;
using soothsayer.Infrastructure;
using soothsayer.Infrastructure.IO;
using soothsayer.Oracle;
using soothsayer.Scripts;

namespace soothsayer.Migrations
{
    public class InitMigration : IMigration
    {
        private readonly IDatabaseMetadataProvider _databaseMetadataProvider;
        private readonly bool _force;

        public InitMigration(IDatabaseMetadataProvider databaseMetadataProvider, bool force)
        {
            _databaseMetadataProvider = databaseMetadataProvider;
            _force = force;
        }

        public void Migrate(IEnumerable<IStep> migrationSteps, DatabaseVersion currentVersion, long? targetVersion, IScriptRunner scriptRunner, string schema, string tablespace)
        {
            if (!_databaseMetadataProvider.SchemaExists(schema))
            {
                InitialiseDatabase(migrationSteps.Select(m => m.ForwardScript), scriptRunner, schema);
            }
        }

        private void InitialiseDatabase(IEnumerable<IScript> initScripts, IScriptRunner scriptRunner, string schema)
        {
            Output.Warn("*** Target database could not be found and needs to be initialised before the migration can be run... ***");
            Output.EmptyLine();

            var initScriptsList = initScripts as IList<IScript> ?? initScripts.ToList();
            if (initScriptsList.IsNullOrEmpty())
            {
                Output.Warn("No initialisation scripts found to create the database. Nothing will be done.");
            }
            else
            {
                Output.Info("The following scripts will be used to create and initialise the tablespace and schema:");
                foreach (var script in initScriptsList)
                {
                    Output.Info(script.Name, 1);
                }

                Output.EmptyLine();
                Output.Warn("Note: soothsayer assumes that the tablespace and schema share the same name, the versioning table will be created in tablespace '{0}'".FormatWith(schema));

                Prompt.ForAnyKey("Press any key to start the 'init' migration. Ctrl-C to abort.");

                foreach (var script in initScriptsList)
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

                Output.Info("Initialisation scripts completed.");
            }

            Output.EmptyLine();
        }
    }
}
