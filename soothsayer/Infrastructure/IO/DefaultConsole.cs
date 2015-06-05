using System;

namespace soothsayer.Infrastructure.IO
{
    public class DefaultConsole : IConsole
    {
        public void ReadKey()
        {
            Console.ReadKey();
        }
    }
}
