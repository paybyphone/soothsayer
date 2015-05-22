using System;
using System.Collections.Generic;
using System.Linq;

namespace soothsayer.Infrastructure.IO
{
    internal class SecureConsole : ISecureConsole
    {
        public string ReadLine(char maskCharacter)
        {
            const int enterCharacter = 13, backspaceCharacter = 8, ctrlBackspaceCharacter = 127;
            int[] filteredCharacters = { 0, 27, 9, 10 };

            var password = new Stack<char>();
            char readCharacter;

            while ((readCharacter = Console.ReadKey(true).KeyChar) != enterCharacter)
            {
                if (readCharacter == backspaceCharacter)
                {
                    if (password.Count > 0)
                    {
                        Console.Write("\b \b");
                        password.Pop();
                    }
                }
                else if (readCharacter == ctrlBackspaceCharacter)
                {
                    while (password.Count > 0)
                    {
                        Console.Write("\b \b");
                        password.Pop();
                    }
                }
                else if (filteredCharacters.Count(x => readCharacter == x) > 0)
                {
                    //do nothing
                }
                else
                {
                    password.Push(readCharacter);
                    Console.Write(maskCharacter);
                }
            }

            Console.WriteLine();

            return new string(password.Reverse().ToArray());
        }
    }
}
