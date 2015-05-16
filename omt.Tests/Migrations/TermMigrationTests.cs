using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using omt.Infrastructure;
using omt.Migrations;
using omt.Scripts;

namespace omt.Tests.Migrations
{
    [TestFixture]
    public class TermMigrationTests
    {
        public List<IScript> SomeScripts = new List<IScript> { new Script("foo", 1), new Script("bar", 2) };

        private Mock<IDatabaseMetadataProvider> _mockMetadataProvider;
        private Mock<IScriptRunner> _mockScriptRunner;

        [SetUp]
        public void SetUp()
        {
            Prompt.ConsoleProvider = new Mock<IConsole>().Object;

            _mockMetadataProvider = new Mock<IDatabaseMetadataProvider>();
            _mockScriptRunner = new Mock<IScriptRunner>();
        }

        [Test]
        public void when_there_are_no_migration_scripts_then_none_are_ever_executed()
        {
            _mockMetadataProvider.Setup(m => m.SchemaExists(It.IsAny<string>())).Returns(false);

            var migration = new TermMigration(_mockMetadataProvider.Object, false);
            migration.Migrate(Enumerable.Empty<IScript>(), null, null, _mockScriptRunner.Object, "someSchema", "someTablespace");

            _mockScriptRunner.Verify(m => m.Execute(It.IsAny<IScript>()), Times.Never);
        }
    }
}
