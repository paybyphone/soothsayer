using System.Collections.Generic;
using omt.Scripts;

namespace omt.Scanners
{
    public interface IScriptScanner
    {
        IEnumerable<Script> Scan(string scriptPath, string environment);
    }
}