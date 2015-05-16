using System.Collections.Generic;
using omt.Scripts;

namespace omt.Migrations
{
    public interface IMigration
    {
        void Migrate(IEnumerable<IScript> migrationScripts, DatabaseVersion currentVersion, long? targetVersionNumber, IScriptRunner scriptRunner, string schema, string tablespace);
    }
}