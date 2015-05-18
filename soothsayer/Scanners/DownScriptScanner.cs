using System.Collections.Generic;
using System.IO;
using System.Linq;
using soothsayer.Infrastructure;
using soothsayer.Scripts;

namespace soothsayer.Scanners
{
    public class DownScriptScanner : IScriptScanner
    {
        public IEnumerable<Script> Scan(string downScriptPath, string environment)
        {
            var scriptFiles = Directory.GetFiles(downScriptPath, "*.sql")
                                       .OrderByDescending(s => s);

			var filteredScriptFiles = scriptFiles.Where(s =>
                s.FileName().Substring(3).Matches(FilePattern.ForEnvironment(environment))
                || s.FileName().Substring(3).Matches(FilePattern.NoEnvironment));

			return filteredScriptFiles.Select(s => new Script(s, ParseRollbackVersion(s.FileName())));
        }

        private long ParseRollbackVersion(string scriptFileName)
        {
            var scriptFileNameWithoutRollbackPrefix = scriptFileName.Substring(3);
            return ParseVersion(scriptFileNameWithoutRollbackPrefix);
        }

        private long ParseVersion(string scriptFileName)
        {
            var firstUnderscorePosition = scriptFileName.IndexOf('_');
            return long.Parse(scriptFileName.Substring(0, firstUnderscorePosition));
        }
    }
}
