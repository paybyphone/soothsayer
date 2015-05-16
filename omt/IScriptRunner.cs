using omt.Scripts;

namespace omt
{
    public interface IScriptRunner
    {
        void Execute(IScript script);
    }
}