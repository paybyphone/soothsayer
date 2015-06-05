using System.Data;

namespace soothsayer.Oracle
{
    public class OracleVersionRespositoryFactory : IVersionRespositoryFactory
    {
        public IVersionRespository Create(IDbConnection connection)
        {
            return new OracleVersionRespository(connection);
        }
    }
}
