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
        private readonly IAppliedScriptsRepository _appliedScriptsRepository;
        private readonly bool _force;

        public UpMigration(IVersionRespository versionRespository, IAppliedScriptsRepository appliedScriptsRepository, bool force)
        {
            _versionRespository = versionRespository;
            _appliedScriptsRepository = appliedScriptsRepository;
            _force = force;
        }

        public void Migrate(IEnumerable<IStep> migrationSteps, DatabaseVersion currentVersion, long? targetVersion, IScriptRunner scriptRunner, string schema, string tablespace)
        {
            var steps = migrationSteps as IList<IStep> ?? migrationSteps.ToList();

            var forwardScripts = steps.Select(m => m.ForwardScript).ToList();
            var backwardScripts = steps.Select(m => m.BackwardScript).ToList();

            var applicableScripts = forwardScripts.Where(s => currentVersion.IsNull() || s.Version > currentVersion.Version)
                    .Where(s => !targetVersion.HasValue || s.Version <= targetVersion).ToArray();

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

                    Output.Text("Adding script contents for script '{0}' to applied scripts table".FormatWith(script.Name));
                    _appliedScriptsRepository.InsertAppliedScript(script.AsDatabaseVersion(), schema, script, backwardScripts.FirstOrDefault(m => m.IsNotNull() && m.Version == script.Version));
                }
            }
        }
    }
}
