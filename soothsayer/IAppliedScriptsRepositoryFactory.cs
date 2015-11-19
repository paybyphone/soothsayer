using System.Data;

namespace soothsayer
{
    public interface IAppliedScriptsRepositoryFactory
    {
        IAppliedScriptsRepository Create(IDbConnection connection);
    }
}
