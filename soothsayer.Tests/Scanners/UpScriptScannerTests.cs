using System;
using System.Linq;
using Moq;
using NUnit.Framework;
using soothsayer.Infrastructure;
using soothsayer.Infrastructure.IO;
using soothsayer.Scanners;

namespace soothsayer.Tests.Scanners
{
    [TestFixture]
    public class UpScriptScannerTests
    {
        protected Mock<IFilesystem> MockFilesystem;
        protected UpScriptScanner Scanner;

        [SetUp]
        public virtual void SetUp()
        {
            MockFilesystem = new Mock<IFilesystem>();
            Scanner = new UpScriptScanner(MockFilesystem.Object);
        }

        [Test]
        public void script_files_are_returned_in_alphabetical_order()
        {
            MockFilesystem.Setup(m => m.GetFiles(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new[] { "03_c.sql", "01_a.sql", "26_z.sql", "05_e.sql" });

            var scriptFiles = Scanner.Scan(Some.Value("folder"), Some.Value("environment")).ToList();
            var expectedOrder = new[] { "01_a.sql", "03_c.sql", "05_e.sql", "26_z.sql" };

            Assert.That(scriptFiles.Count, Is.EqualTo(expectedOrder.Count()));

            for (int i = 0; i < scriptFiles.Count(); i++)
            {
                Assert.That(scriptFiles[i].Name, Is.EqualTo(expectedOrder[i]));
            }
        }

        [Test]
        public void script_files_must_start_with_a_version()
        {
            MockFilesystem.Setup(m => m.GetFiles(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new[] { "noversion.sql" });

            Assert.Throws<InvalidOperationException>(() => Scanner.Scan(Some.Value("folder"), Some.Value("environment")).Enumerate());
        }

        [Test]
        public void script_files_with_no_environment_are_returned()
        {
            MockFilesystem.Setup(m => m.GetFiles(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new[] { "03_c.sql", "01_a.sql", "26_z.env.sql", "05_e.sql" });

            var scriptFiles = Scanner.Scan(Some.Value("folder"), Some.Value("environment")).ToList();
            var expectedScripts = new[] { "01_a.sql", "03_c.sql", "05_e.sql" };

            Assert.That(scriptFiles.Count, Is.EqualTo(expectedScripts.Count()));
            Assert.That(expectedScripts.All(e => scriptFiles.Any(s => s.Name.Equals(e))));
        }

        [Test]
        public void script_files_with_matching_environment_are_returned()
        {
            MockFilesystem.Setup(m => m.GetFiles(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new[] { "03_c.notmatching.sql", "01_a.notmatching.sql", "26_z.environment.sql", "05_e.notmatching.sql" });

            var scriptFiles = Scanner.Scan(Some.Value("folder"), Some.Value("environment")).ToList();
            var expectedScripts = new[] { "26_z.environment.sql" };

            Assert.That(scriptFiles.Count, Is.EqualTo(expectedScripts.Count()));
            Assert.That(expectedScripts.All(e => scriptFiles.Any(s => s.Name.Equals(e))));
        }
    }
}