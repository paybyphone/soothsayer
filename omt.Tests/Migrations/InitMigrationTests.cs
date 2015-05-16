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
    public class InitMigrationTests
    {
        public List<IScript> SomeScripts = new List<IScript> { new Script("foo", 1), new Script("bar", 2) };

        private Mock<IDatabaseMetadataProvider> _mockMetadataProvider;
        private Mock<IVersionRespository> _mockVersionRepository;
        private Mock<IScriptRunner> _mockScriptRunner;

        [SetUp]
        public void SetUp()
        {
            Prompt.ConsoleProvider = new Mock<IConsole>().Object;

            _mockMetadataProvider = new Mock<IDatabaseMetadataProvider>();
            _mockVersionRepository = new Mock<IVersionRespository>();
            _mockScriptRunner = new Mock<IScriptRunner>();
        }

        [Test]
        public void when_there_are_no_migration_scripts_then_none_are_ever_executed()
        {
            var migration = new InitMigration(_mockMetadataProvider.Object, _mockVersionRepository.Object, false);
            migration.Migrate(Enumerable.Empty<IScript>(), null, null, _mockScriptRunner.Object, Some.Value<string>(), Some.Value<string>());

            _mockScriptRunner.Verify(m => m.Execute(It.IsAny<IScript>()), Times.Never);
        }

        [Test]
        public void for_each_migration_script_upgraded_no_versioning_is_necessary()
        {
            var migration = new InitMigration(_mockMetadataProvider.Object, _mockVersionRepository.Object, false);
            migration.Migrate(SomeScripts, null, null, _mockScriptRunner.Object, Some.Value<string>(), Some.Value<string>());

            _mockVersionRepository.Verify(m => m.InsertVersion(SomeScripts[0].AsDatabaseVersion(), It.IsAny<string>()), Times.Never);
            _mockVersionRepository.Verify(m => m.InsertVersion(SomeScripts[1].AsDatabaseVersion(), It.IsAny<string>()), Times.Never);
        }
    }
}
