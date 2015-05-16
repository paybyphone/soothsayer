using System;

namespace omt.Infrastructure
{
    public class DefaultConsole : IConsole
    {
        public void ReadKey()
        {
            Console.ReadKey();
        }
    }
}