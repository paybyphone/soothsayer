using ColoredConsole;

namespace soothsayer.Infrastructure.IO
{
    public interface IColorConsole
    {
        void WriteLine(params ColorToken[] tokens);
    }
}