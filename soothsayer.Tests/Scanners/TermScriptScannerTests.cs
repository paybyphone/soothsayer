using Moq;
using NUnit.Framework;
using soothsayer.Infrastructure;
using soothsayer.Infrastructure.IO;
using soothsayer.Scanners;

namespace soothsayer.Tests.Scanners
{
    [TestFixture]
    public class TermScriptScannerTests : DownScriptScannerTests
    {
        [SetUp]
        public override void SetUp()
        {
            MockFilesystem = new Mock<IFilesystem>();
            Scanner = new TermScriptScanner(MockFilesystem.Object);
        }
    }
}