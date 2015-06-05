using System.Collections.Generic;
using soothsayer.Scripts;

namespace soothsayer.Migrations
{
    public interface IMigration
    {
        void Migrate(IEnumerable<IScript> migrationScripts, DatabaseVersion currentVersion, long? targetVersionNumber, IScriptRunner scriptRunner, string schema, string tablespace);
    }
}
