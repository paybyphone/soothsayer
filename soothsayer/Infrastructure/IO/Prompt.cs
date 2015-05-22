using ColoredConsole;

namespace soothsayer.Infrastructure.IO
{
    public static class Prompt
    {
        public static bool NoPrompts = false;
        public static IConsole ConsoleProvider = new DefaultConsole();

        public static void ForAnyKey(string promptMessage)
        {
            ColorConsole.WriteLine(promptMessage.White());

            if (!NoPrompts)
            {
                ConsoleProvider.ReadKey();
            }
        }
    }
}
