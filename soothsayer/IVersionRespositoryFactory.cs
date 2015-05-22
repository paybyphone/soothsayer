using System.Data;

namespace soothsayer
{
    public interface IVersionRespositoryFactory
    {
        IVersionRespository Create(IDbConnection connection);
    }
}
