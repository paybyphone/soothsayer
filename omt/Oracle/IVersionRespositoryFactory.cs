using System.Data;

namespace omt.Oracle
{
    public interface IVersionRespositoryFactory
    {
        IVersionRespository Create(IDbConnection connection);
    }
}