using System.Collections.Generic;
using System.Linq;
using soothsayer.Infrastructure;
using soothsayer.Infrastructure.IO;
using soothsayer.Oracle;
using soothsayer.Scripts;

namespace soothsayer.Migrations
{
    public class UpMigration : IMigration
    {
        private readonly IVersionRespository _versionRespository;
        private readonly bool _force;

        public UpMigration(IVersionRespository versionRespository, bool force)
        {
            _versionRespository = versionRespository;
            _force = force;
        }

        public void Migrate(IEnumerable<IScript> migrationScripts, DatabaseVersion currentVersion, long? targetVersion, IScriptRunner scriptRunner, string schema, string tablespace)
        {
            var applicableScripts = migrationScripts.Where(s => currentVersion.IsNull() || s.Version > currentVersion.Version)
                    .Where(s => !targetVersion.HasValue || s.Version <= targetVersion).ToArray();

            if (applicableScripts.IsEmpty())
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
                Prompt.ForAnyKey("Press any key to start the 'up' migration. Ctrl-C to abort.");

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

                    Output.Text("Adding version {0} for script '{1}' to version table".FormatWith(script.Version, script.Name));
                    _versionRespository.InsertVersion(script.AsDatabaseVersion(), schema);
                }
            }
        }
    }
}
