using System.Data;

namespace omt.Oracle
{
    public class OracleVersionRespositoryFactory : IVersionRespositoryFactory
    {
        public IVersionRespository Create(IDbConnection connection)
        {
            return new OracleVersionRespository(connection);
        }
    }
}