using System.Collections.Generic;
using System.IO;

namespace soothsayer.Scripts
{
    public class ScriptReader
    {
        public IEnumerable<string> GetContents(string path)
        {
            return File.ReadAllLines(path);
        }
    }
}