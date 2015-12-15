using System;
using System.Collections.Generic;
using System.Linq;
using soothsayer.Infrastructure;
using soothsayer.Infrastructure.IO;
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

        public IEnumerable<Script> Scan(string upScriptPath, params string[] environment)
        {
            var scriptFiles = _filesystem.GetFiles(upScriptPath, "*.sql")
                                         .OrderBy(scriptPath => scriptPath);

			var filteredScriptFiles = scriptFiles.Where(scriptPath =>
                        environment.Any(e => scriptPath.FileName().Matches(FilePattern.ForEnvironment(e)))
                        || scriptPath.FileName().Matches(FilePattern.NoEnvironment));

			return filteredScriptFiles.Select(scriptPath => new Script(scriptPath, ParseVersion(scriptPath.FileName())));
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
