using System.Data;

namespace soothsayer
{
    public interface IDatabaseMetadataProviderFactory
    {
        IDatabaseMetadataProvider Create(IDbConnection connection);
    }
}
