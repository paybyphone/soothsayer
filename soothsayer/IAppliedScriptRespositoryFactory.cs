using System.Data;

namespace soothsayer
{
    public interface IAppliedScriptRespositoryFactory
    {
        IAppliedScriptsRepository Create(IDbConnection connection);
    }
}
