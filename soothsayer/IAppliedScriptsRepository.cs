using soothsayer.Scripts;

namespace soothsayer
{
    public interface IAppliedScriptsRepository
    {
        IScript GetAppliedScript(DatabaseVersion version, string schema);
        IScript GetRollbackScript(DatabaseVersion version, string schema);
        void InsertAppliedScript(DatabaseVersion version, string schema, IScript script, IScript rollbackScript = null);
        void InitialiseAppliedScriptsTable(string schema, string tablespace = null);
    }
}