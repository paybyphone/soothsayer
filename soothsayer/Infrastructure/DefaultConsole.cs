using System;

namespace soothsayer.Infrastructure
{
    public class DefaultConsole : IConsole
    {
        public void ReadKey()
        {
            Console.ReadKey();
        }
    }
}
