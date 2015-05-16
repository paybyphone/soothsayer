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
    public class DownMigrationTests
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
            _mockMetadataProvider.Setup(m => m.SchemaExists(It.IsAny<string>())).Returns(false);

            var migration = new DownMigration(_mockMetadataProvider.Object, _mockVersionRepository.Object, false);
            migration.Migrate(Enumerable.Empty<IScript>(), null, null, _mockScriptRunner.Object, Some.Value<string>(), Some.Value<string>());

            _mockScriptRunner.Verify(m => m.Execute(It.IsAny<IScript>()), Times.Never);
        }

        [Test]
        public void when_schema_does_not_exist_then_do_not_run_any_migration_scripts()
        {
            _mockMetadataProvider.Setup(m => m.SchemaExists(It.IsAny<string>())).Returns(false);

            var migration = new DownMigration(_mockMetadataProvider.Object, _mockVersionRepository.Object, false);
            migration.Migrate(SomeScripts, null, null, _mockScriptRunner.Object, Some.Value<string>(), Some.Value<string>());

            _mockScriptRunner.Verify(m => m.Execute(It.IsAny<IScript>()), Times.Never);
        }

        [Test]
        public void when_schema_exists_but_there_are_no_recorded_versions_then_do_not_run_any_migration_scripts()
        {
            _mockMetadataProvider.Setup(m => m.SchemaExists(It.IsAny<string>())).Returns(true);
            _mockVersionRepository.Setup(m => m.GetCurrentVersion(It.IsAny<string>())).Returns((DatabaseVersion)null);

            var migration = new DownMigration(_mockMetadataProvider.Object, _mockVersionRepository.Object, false);
            migration.Migrate(SomeScripts, null, null, _mockScriptRunner.Object, Some.Value<string>(), Some.Value<string>());

            _mockScriptRunner.Verify(m => m.Execute(It.IsAny<IScript>()), Times.Never);
        }

        [Test]
        public void when_schema_exists_and_there_are_recorded_versions_then_run_migration_scripts()
        {
            _mockMetadataProvider.Setup(m => m.SchemaExists(It.IsAny<string>())).Returns(true);
            _mockVersionRepository.Setup(m => m.GetCurrentVersion(It.IsAny<string>())).Returns(new DatabaseVersion(1234, Some.Value<string>()));

            var migration = new DownMigration(_mockMetadataProvider.Object, _mockVersionRepository.Object, false);
            migration.Migrate(SomeScripts, null, null, _mockScriptRunner.Object, Some.Value<string>(), Some.Value<string>());

            _mockScriptRunner.Verify(m => m.Execute(It.IsAny<IScript>()), Times.Exactly(2));
        }

        [Test]
        public void for_each_migration_script_downgraded_their_version_is_removed()
        {
            _mockMetadataProvider.Setup(m => m.SchemaExists(It.IsAny<string>())).Returns(true);
            _mockVersionRepository.Setup(m => m.GetCurrentVersion(It.IsAny<string>())).Returns(new DatabaseVersion(1234, Some.Value<string>()));

            var migration = new DownMigration(_mockMetadataProvider.Object, _mockVersionRepository.Object, false);
            migration.Migrate(SomeScripts, null, null, _mockScriptRunner.Object, Some.Value<string>(), Some.Value<string>());

            _mockVersionRepository.Verify(m => m.RemoveVersion(SomeScripts[0].AsDatabaseVersion(), It.IsAny<string>()), Times.Once);
            _mockVersionRepository.Verify(m => m.RemoveVersion(SomeScripts[1].AsDatabaseVersion(), It.IsAny<string>()), Times.Once);
        }
    }
}
