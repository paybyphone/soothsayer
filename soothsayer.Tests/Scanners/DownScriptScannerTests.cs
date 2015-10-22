using System;
using System.Linq;
using Moq;
using NUnit.Framework;
using soothsayer.Infrastructure.IO;
using soothsayer.Scanners;

namespace soothsayer.Tests.Scanners
{
    [TestFixture]
    public class DownScriptScannerTests
    {
        protected Mock<IFilesystem> MockFilesystem;
        protected IScriptScanner Scanner;

        [SetUp]
        public virtual void SetUp()
        {
            MockFilesystem = new Mock<IFilesystem>();
            Scanner = new DownScriptScanner(MockFilesystem.Object);
        }

        [Test]
        public void script_files_are_returned_in_reverse_alphabetical_order()
        {
            MockFilesystem.Setup(m => m.GetFiles(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new[] { "RB_03_c.sql", "RB_01_a.sql", "RB_26_z.sql", "RB_05_e.sql" });

            var scriptFiles = Scanner.Scan(Some.Value("folder"), Some.Value("environment")).ToList();
            var expectedOrder = new[] { "RB_26_z.sql", "RB_05_e.sql", "RB_03_c.sql", "RB_01_a.sql" };

            Assert.That(scriptFiles.Count, Is.EqualTo(expectedOrder.Count()));

            for (int i = 0; i < scriptFiles.Count(); i++)
            {
                Assert.That(scriptFiles[i].Name, Is.EqualTo(expectedOrder[i]));
            }
        }

        [Test]
        public void script_files_must_start_with_a_rollback_indicator()
        {
            MockFilesystem.Setup(m => m.GetFiles(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new[] { "noversion.sql" });

            Assert.Throws<InvalidOperationException>(() => Scanner.Scan(Some.Value("folder"), Some.Value("environment")).Enumerate());
        }

        [Test]
        public void script_files_must_start_with_a_version()
        {
            MockFilesystem.Setup(m => m.GetFiles(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new[] { "RB_noversion.sql" });

            Assert.Throws<InvalidOperationException>(() => Scanner.Scan(Some.Value("folder"), Some.Value("environment")).Enumerate());
        }

        [Test]
        public void script_files_with_no_environment_are_returned()
        {
            MockFilesystem.Setup(m => m.GetFiles(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new[] { "RB_03_c.sql", "RB_01_a.sql", "RB_26_z.env.sql", "RB_05_e.sql" });

            var scriptFiles = Scanner.Scan(Some.Value("folder"), Some.Value("environment")).ToList();
            var expectedScripts = new[] { "RB_01_a.sql", "RB_03_c.sql", "RB_05_e.sql" };

            Assert.That(scriptFiles.Count, Is.EqualTo(expectedScripts.Count()));
            Assert.That(expectedScripts.All(e => scriptFiles.Any(s => s.Name.Equals(e))));
        }

        [Test]
        public void script_files_with_matching_environment_are_returned()
        {
            MockFilesystem.Setup(m => m.GetFiles(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new[] { "RB_03_c.notmatching.sql", "RB_01_a.notmatching.sql", "RB_26_z.environment.sql", "RB_05_e.notmatching.sql" });

            var scriptFiles = Scanner.Scan(Some.Value("folder"), Some.Value("environment")).ToList();
            var expectedScripts = new[] { "RB_26_z.environment.sql" };

            Assert.That(scriptFiles.Count, Is.EqualTo(expectedScripts.Count()));
            Assert.That(expectedScripts.All(e => scriptFiles.Any(s => s.Name.Equals(e))));
        }

        [Test]
        public void script_files_which_matches_any_of_several_environments_are_returned()
        {
            MockFilesystem.Setup(m => m.GetFiles(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new[] { "RB_03_c.notmatching.sql", "RB_01_a.notmatching.sql", "RB_26_z.environment1.sql", "RB_05_e.environment2.sql" });

            var scriptFiles = Scanner.Scan(Some.Value("folder"), Some.Value("environment1"), Some.Value("environment2")).ToList();
            var expectedScripts = new[] { "RB_26_z.environment1.sql", "RB_05_e.environment2.sql" };

            Assert.That(scriptFiles.Count, Is.EqualTo(expectedScripts.Count()));
            Assert.That(expectedScripts.All(e => scriptFiles.Any(s => s.Name.Equals(e))));
        }
    }
}