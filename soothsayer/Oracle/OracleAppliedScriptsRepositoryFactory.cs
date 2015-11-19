using System.Data;

namespace soothsayer.Oracle
{
    public class OracleAppliedScriptsRepositoryFactory : IAppliedScriptsRepositoryFactory
    {
        public IAppliedScriptsRepository Create(IDbConnection connection)
        {
            return new OracleAppliedScriptsRepository(connection);
        }
    }
}