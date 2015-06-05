using System.Collections.Generic;
using Moq;
using soothsayer.Scanners;
using soothsayer.Scripts;

namespace soothsayer.Tests
{
    public class MockScriptScannerFactory : IScriptScannerFactory
    {
        private readonly Dictionary<string, Mock<IScriptScanner>> _mockScriptScanners = new Dictionary<string, Mock<IScriptScanner>>
            {
                { ScriptFolders.Init, new Mock<IScriptScanner>() },
                { ScriptFolders.Up, new Mock<IScriptScanner>() },
                { ScriptFolders.Down, new Mock<IScriptScanner>() },
                { ScriptFolders.Term, new Mock<IScriptScanner>() }
            };

        public IEnumerable<Mock<IScriptScanner>> GetMocks()
        {
            return _mockScriptScanners.Values;
        }

        public Mock<IScriptScanner> GetMock(string scriptFolder)
        {
            return _mockScriptScanners[scriptFolder];
        }

        public IScriptScanner Create(string scriptFolder)
        {
            return _mockScriptScanners[scriptFolder].Object;
        }
    }
}