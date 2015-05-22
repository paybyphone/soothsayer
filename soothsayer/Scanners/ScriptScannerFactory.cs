using System;
using System.Collections.Generic;
using soothsayer.Infrastructure;
using soothsayer.Infrastructure.IO;
using soothsayer.Scripts;

namespace soothsayer.Scanners
{
    public class ScriptScannerFactory : IScriptScannerFactory
    {
        private readonly IFilesystem _filesystem;

        private readonly Dictionary<string, Func<IFilesystem, IScriptScanner>> _registeredScannerProviders
            = new Dictionary<string, Func<IFilesystem, IScriptScanner>>
            {
                { ScriptFolders.Up, fs => new UpScriptScanner(fs) },
                { ScriptFolders.Down, fs => new DownScriptScanner(fs) },
                { ScriptFolders.Init, fs => new InitScriptScanner(fs) },
                { ScriptFolders.Term, fs => new TermScriptScanner(fs) }
            };

        public ScriptScannerFactory(IFilesystem filesystem)
        {
            _filesystem = filesystem;
        }

        public IScriptScanner Create(string scriptFolder)
        {
            var provider = _registeredScannerProviders[scriptFolder];
            return provider(_filesystem);
        }
    }
}