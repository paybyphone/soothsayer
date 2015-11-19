using System.Collections.Generic;
using soothsayer.Scripts;

namespace soothsayer
{
    public interface IAppliedScriptsRepository
    {
        IEnumerable<IManoeuvre> GetAppliedScripts(string schema);
        void InsertAppliedScript(long version, string schema, IScript script, IScript rollbackScript = null);
        void RemoveAppliedScript(long version, string schema);

        bool AppliedScriptsTableExists(string schema);
        void InitialiseAppliedScriptsTable(string schema, string tablespace = null);
    }
}