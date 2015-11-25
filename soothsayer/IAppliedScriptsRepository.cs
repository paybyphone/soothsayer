using System.Collections.Generic;
using soothsayer.Scripts;

namespace soothsayer
{
    public interface IAppliedScriptsRepository
    {
        IEnumerable<IStep> GetAppliedScripts(string schema);
        void InsertAppliedScript(DatabaseVersion version, string schema, IScript script, IScript rollbackScript = null);
        void RemoveAppliedScript(DatabaseVersion version, string schema);

        bool AppliedScriptsTableExists(string schema);
        void InitialiseAppliedScriptsTable(string schema, string tablespace = null);
    }
}