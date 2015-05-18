using System.Collections.Generic;
using System.IO;
using System.Linq;
using soothsayer.Infrastructure;
using soothsayer.Scripts;

namespace soothsayer.Scanners
{
    public class UpScriptScanner : IScriptScanner
    {
        public IEnumerable<Script> Scan(string upScriptPath, string environment)
        {
            var scriptFiles = Directory.GetFiles(upScriptPath, "*.sql")
                                       .OrderBy(s => s);

			var filteredScriptFiles = scriptFiles.Where(s =>
                s.FileName().Matches(FilePattern.ForEnvironment(environment))
                || s.FileName().Matches(FilePattern.NoEnvironment));

			return filteredScriptFiles.Select(s => new Script(s, ParseVersion(s.FileName())));
        }

        private long ParseVersion(string scriptFileName)
        {
            var firstUnderscorePosition = scriptFileName.IndexOf('_');
            return long.Parse(scriptFileName.Substring(0, firstUnderscorePosition));
        }
    }
}
