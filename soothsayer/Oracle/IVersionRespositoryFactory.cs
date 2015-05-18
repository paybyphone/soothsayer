using System.Data;

namespace soothsayer.Oracle
{
    public interface IVersionRespositoryFactory
    {
        IVersionRespository Create(IDbConnection connection);
    }
}
