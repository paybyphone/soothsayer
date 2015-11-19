using soothsayer.Scripts;

namespace soothsayer
{
    public interface IAppliedScriptsRepository
    {
        IScript GetAppliedScript(long version, string schema);
        IScript GetRollbackScript(long version, string schema);
        void InsertAppliedScript(long version, string schema, IScript script, IScript rollbackScript = null);
        bool AppliedScriptsTableExists(string schema);
        void RemoveAppliedScript(long version, string schema);
        void InitialiseAppliedScriptsTable(string schema, string tablespace = null);
    }
}