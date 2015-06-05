using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using soothsayer.Infrastructure.IO;
using soothsayer.Migrations;
using soothsayer.Oracle;
using soothsayer.Scripts;

namespace soothsayer.Tests.Migrations
{
    [TestFixture]
    public class UpMigrationTests
    {
        public List<IScript> SomeScripts = new List<IScript> { new Script("foo", 1), new Script("bar", 2), new Script("baz", 3) };

        private Mock<IVersionRespository> _mockVersionRepository;
        private Mock<IScriptRunner> _mockScriptRunner;

        [SetUp]
        public void SetUp()
        {
            Prompt.ConsoleProvider = new Mock<IConsole>().Object;

            _mockVersionRepository = new Mock<IVersionRespository>();
            _mockScriptRunner = new Mock<IScriptRunner>();
        }

        [Test]
        public void when_there_are_no_migration_scripts_then_none_are_ever_executed()
        {
            var migration = new UpMigration(_mockVersionRepository.Object, false);
            migration.Migrate(Enumerable.Empty<IScript>(), null, null, _mockScriptRunner.Object, Some.String(), Some.String());

            _mockScriptRunner.Verify(m => m.Execute(It.IsAny<IScript>()), Times.Never);
        }

        [Test]
        public void for_each_migration_script_upgraded_their_version_is_added()
        {
            var migration = new UpMigration(_mockVersionRepository.Object, false);
            migration.Migrate(SomeScripts, null, null, _mockScriptRunner.Object, Some.String(), Some.String());

            _mockVersionRepository.Verify(m => m.InsertVersion(SomeScripts[0].AsDatabaseVersion(), It.IsAny<string>()), Times.Once);
            _mockVersionRepository.Verify(m => m.InsertVersion(SomeScripts[1].AsDatabaseVersion(), It.IsAny<string>()), Times.Once);
            _mockVersionRepository.Verify(m => m.InsertVersion(SomeScripts[2].AsDatabaseVersion(), It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void if_a_migration_script_fails_then_the_following_migration_scripts_do_not_run()
        {
            var migration = new UpMigration(_mockVersionRepository.Object, force: false);

            _mockScriptRunner.Setup(m => m.Execute(SomeScripts[1])).Throws(new SqlPlusException());

            Ignore.Exception(() => migration.Migrate(SomeScripts, null, null, _mockScriptRunner.Object, Some.String(), Some.String()));

            _mockVersionRepository.Verify(m => m.InsertVersion(SomeScripts[0].AsDatabaseVersion(), It.IsAny<string>()), Times.Once);
            _mockVersionRepository.Verify(m => m.InsertVersion(SomeScripts[1].AsDatabaseVersion(), It.IsAny<string>()), Times.Never);
            _mockVersionRepository.Verify(m => m.InsertVersion(SomeScripts[2].AsDatabaseVersion(), It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void if_force_is_specified_then_the_migration_scripts_will_still_run()
        {
            var migration = new UpMigration(_mockVersionRepository.Object, force: true);

            _mockScriptRunner.Setup(m => m.Execute(SomeScripts[1])).Throws(new SqlPlusException());

            Ignore.Exception(() => migration.Migrate(SomeScripts, null, null, _mockScriptRunner.Object, Some.String(), Some.String()));

            _mockVersionRepository.Verify(m => m.InsertVersion(SomeScripts[0].AsDatabaseVersion(), It.IsAny<string>()), Times.Once);
            _mockVersionRepository.Verify(m => m.InsertVersion(SomeScripts[1].AsDatabaseVersion(), It.IsAny<string>()), Times.Once);
            _mockVersionRepository.Verify(m => m.InsertVersion(SomeScripts[2].AsDatabaseVersion(), It.IsAny<string>()), Times.Once);
        }
    }
}
