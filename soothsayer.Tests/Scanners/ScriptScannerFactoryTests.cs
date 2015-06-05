using Moq;
using NUnit.Framework;
using soothsayer.Infrastructure;
using soothsayer.Infrastructure.IO;
using soothsayer.Scanners;
using soothsayer.Scripts;

namespace soothsayer.Tests.Scanners
{
    [TestFixture]
    public class ScriptScannerFactoryTests
    {
        private Mock<IFilesystem> _mockFilesystem;
        private ScriptScannerFactory _factory;

        [SetUp]
        public void SetUp()
        {
            _mockFilesystem = new Mock<IFilesystem>();
            _factory = new ScriptScannerFactory(_mockFilesystem.Object);
        }

        [Test]
        public void up_script_folder_uses_the_up_script_scanner()
        {
            var scanner = _factory.Create(ScriptFolders.Up);
            Assert.That(scanner, Is.TypeOf<UpScriptScanner>());
        }

        [Test]
        public void down_script_folder_uses_the_down_script_scanner()
        {
            var scanner = _factory.Create(ScriptFolders.Down);
            Assert.That(scanner, Is.TypeOf<DownScriptScanner>());
        }

        [Test]
        public void init_script_folder_uses_the_init_script_scanner()
        {
            var scanner = _factory.Create(ScriptFolders.Init);
            Assert.That(scanner, Is.TypeOf<InitScriptScanner>());
        }

        [Test]
        public void term_script_folder_uses_the_term_script_scanner()
        {
            var scanner = _factory.Create(ScriptFolders.Term);
            Assert.That(scanner, Is.TypeOf<TermScriptScanner>());
        }
    }
}