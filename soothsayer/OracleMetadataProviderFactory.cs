using System.Data;
using soothsayer.Oracle;

namespace soothsayer
{
    public class OracleMetadataProviderFactory : IDatabaseMetadataProviderFactory
    {
        public IDatabaseMetadataProvider Create(IDbConnection connection)
        {
            return new OracleMetadataProvider(connection);
        }
    }
}