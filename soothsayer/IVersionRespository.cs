using System.Collections.Generic;

namespace soothsayer
{
    public interface IVersionRespository
    {
        IEnumerable<DatabaseVersion> GetAllVersions(string schema);
        DatabaseVersion GetCurrentVersion(string schema);
        bool ContainsVersion(DatabaseVersion version, string schema);
        void InsertVersion(DatabaseVersion version, string schema);
        void RemoveVersion(DatabaseVersion version, string schema);
        void InitialiseVersioningTable(string schema, string tablespace = null);
        void EnsureVersioningTableIsInitialised(string schema, string tablespace = null);
    }
}
