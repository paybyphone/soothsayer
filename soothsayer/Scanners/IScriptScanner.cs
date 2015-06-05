using System.Collections.Generic;
using soothsayer.Scripts;

namespace soothsayer.Scanners
{
    public interface IScriptScanner
    {
        IEnumerable<Script> Scan(string scriptPath, string environment);
    }
}
