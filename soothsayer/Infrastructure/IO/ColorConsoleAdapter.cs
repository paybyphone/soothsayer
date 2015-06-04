using ColoredConsole;

namespace soothsayer.Infrastructure.IO
{
    public class ColorConsoleAdapter : IColorConsole
    {
        public void WriteLine(params ColorToken[] tokens)
        {
            ColorConsole.WriteLine(tokens);
        }
    }
}