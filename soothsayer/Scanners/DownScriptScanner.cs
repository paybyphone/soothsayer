using System;
using System.Collections.Generic;
using System.Linq;
using soothsayer.Infrastructure;
using soothsayer.Scripts;

namespace soothsayer.Scanners
{
    public class DownScriptScanner : IScriptScanner
    {
        private readonly IFilesystem _filesystem;

        public DownScriptScanner(IFilesystem filesystem)
        {
            _filesystem = filesystem;
        }

        public IEnumerable<Script> Scan(string downScriptPath, string environment)
        {
            var scriptFiles = _filesystem.GetFiles(downScriptPath, "*.sql")
                                       .OrderByDescending(s => s);

			var filteredScriptFiles = scriptFiles.Where(s =>
                s.FileName().Substring(3).Matches(FilePattern.ForEnvironment(environment))
                || s.FileName().Substring(3).Matches(FilePattern.NoEnvironment));

			return filteredScriptFiles.Select(s => new Script(s, ParseRollbackVersion(s.FileName())));
        }

        private long ParseRollbackVersion(string scriptFileName)
        {
            if (!scriptFileName.StartsWith("RB_"))
            {
                throw new InvalidOperationException("scripts must have a rollback prefix 'RB_'");
            }

            var scriptFileNameWithoutRollbackPrefix = scriptFileName.Substring(3);

            return ParseVersion(scriptFileNameWithoutRollbackPrefix);
        }

        private long ParseVersion(string scriptFileName)
        {
            var firstUnderscorePosition = scriptFileName.IndexOf('_');

            if (firstUnderscorePosition < 0)
            {
                throw new InvalidOperationException("scripts must have a version, in the format of 'RB_<numerical version>_<description>[.<environment>].sql'");
            }

            return long.Parse(scriptFileName.Substring(0, firstUnderscorePosition));
        }
    }
}
