using System.Data;

namespace soothsayer.Oracle
{
    public class OracleAppliedScriptsRepositoryFactory : IAppliedScriptRespositoryFactory
    {
        public IAppliedScriptsRepository Create(IDbConnection connection)
        {
            return new OracleAppliedScriptsRepository(connection);
        }
    }
}