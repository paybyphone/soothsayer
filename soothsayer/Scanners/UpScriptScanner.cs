using System;
using System.Collections.Generic;
using System.Linq;
using soothsayer.Infrastructure;
using soothsayer.Scripts;

namespace soothsayer.Scanners
{
    public class UpScriptScanner : IScriptScanner
    {
        private readonly IFilesystem _filesystem;

        public UpScriptScanner(IFilesystem filesystem)
        {
            _filesystem = filesystem;
        }

        public IEnumerable<Script> Scan(string upScriptPath, string environment)
        {
            var scriptFiles = _filesystem.GetFiles(upScriptPath, "*.sql")
                                       .OrderBy(s => s);

			var filteredScriptFiles = scriptFiles.Where(s =>
                s.FileName().Matches(FilePattern.ForEnvironment(environment))
                || s.FileName().Matches(FilePattern.NoEnvironment));

			return filteredScriptFiles.Select(s => new Script(s, ParseVersion(s.FileName())));
        }

        private long ParseVersion(string scriptFileName)
        {
            var firstUnderscorePosition = scriptFileName.IndexOf('_');

            if (firstUnderscorePosition < 0)
            {
                throw new InvalidOperationException("scripts must have a version, in the format of '<numerical version>_<description>[.<environment>].sql'");
            }

            return long.Parse(scriptFileName.Substring(0, firstUnderscorePosition));
        }
    }
}
