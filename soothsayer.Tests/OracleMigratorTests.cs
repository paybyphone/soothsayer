using System.Data;
using ColoredConsole;
using Moq;
using NUnit.Framework;
using soothsayer.Infrastructure;
using soothsayer.Infrastructure.IO;
using soothsayer.Scripts;

namespace soothsayer.Tests
{
    [TestFixture]
    public class OracleMigratorTests
    {
        private Mock<IConnectionFactory> _mockConnectionFactory;
        private Mock<IVersionRespository> _mockVersionRepository;
        private Mock<IVersionRespositoryFactory> _mockVersionRepositoryFactory;
        private Mock<IAppliedScriptsRepository> _mockAppliedScriptRespository;
        private Mock<IAppliedScriptsRepositoryFactory> _mockAppliedScriptRepositoryFactory;
        private Mock<IDatabaseMetadataProvider> _mockDatabaseMetadataProvider;
        private Mock<IDatabaseMetadataProviderFactory> _mockDatabaseMetadataProviderFactory;
        private MockScriptScannerFactory _mockScriptScannerFactory;
        private Mock<IScriptRunner> _mockScriptRunner;
        private Mock<IScriptRunnerFactory> _mockScriptRunnerFactory;
        private Mock<IColorConsole> _mockOutput;
        private OracleMigrator _migrator;

        [SetUp]
        public void SetUp()
        {
            Prompt.ConsoleProvider = new Mock<IConsole>().Object;
            _mockOutput = new Mock<IColorConsole>();
            Output.ConsoleProvider = _mockOutput.Object;

            _mockConnectionFactory = new Mock<IConnectionFactory>();

            _mockVersionRepository = new Mock<IVersionRespository>();
            _mockVersionRepositoryFactory = new Mock<IVersionRespositoryFactory>();
            _mockVersionRepositoryFactory.Setup(m => m.Create(It.IsAny<IDbConnection>())).Returns(_mockVersionRepository.Object);

            _mockAppliedScriptRespository = new Mock<IAppliedScriptsRepository>();
            _mockAppliedScriptRepositoryFactory = new Mock<IAppliedScriptsRepositoryFactory>();
            _mockAppliedScriptRepositoryFactory.Setup(m => m.Create(It.IsAny<IDbConnection>())).Returns(_mockAppliedScriptRespository.Object);

            _mockDatabaseMetadataProvider = new Mock<IDatabaseMetadataProvider>();
            _mockDatabaseMetadataProviderFactory = new Mock<IDatabaseMetadataProviderFactory>();
            _mockDatabaseMetadataProviderFactory.Setup(m => m.Create(It.IsAny<IDbConnection>())).Returns(_mockDatabaseMetadataProvider.Object);

            _mockScriptScannerFactory = new MockScriptScannerFactory();

            _mockScriptRunner = new Mock<IScriptRunner>();
            _mockScriptRunnerFactory = new Mock<IScriptRunnerFactory>();
            _mockScriptRunnerFactory.Setup(m => m.Create(It.IsAny<DatabaseConnectionInfo>())).Returns(_mockScriptRunner.Object);

            _migrator = new OracleMigrator(_mockConnectionFactory.Object, _mockVersionRepositoryFactory.Object, _mockAppliedScriptRepositoryFactory.Object, _mockDatabaseMetadataProviderFactory.Object, _mockScriptScannerFactory, _mockScriptRunnerFactory.Object);
        }

        [Test]
        public void all_supported_script_folders_are_scanned()
        {
            _migrator.Migrate(Some.ConnectionInfo(), Some.MigrationInfo());

            foreach (var mockScriptScanner in _mockScriptScannerFactory.GetMocks())
            {
                mockScriptScanner.Verify(m => m.Scan(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            }
        }

        [Test]
        public void when_an_up_script_is_missing_a_corresponding_down_script_then_a_warning_is_displayed()
        {
            const string upScriptPath = "20150406_scriptpath";
            _mockScriptScannerFactory.GetMock(ScriptFolders.Up).Setup(m => m.Scan(It.IsAny<string>(), It.IsAny<string>())).Returns(new[] { new Script(upScriptPath, 1) });

            _migrator.Migrate(Some.ConnectionInfo(), Some.MigrationInfo());

            _mockOutput.Verify(m => m.WriteLine("The following 'up' scripts do not have a corresponding 'down' script, any rollback may not work as expected:".Yellow()));
            _mockOutput.Verify(m => m.WriteLine("    {0}".FormatWith(upScriptPath).Yellow()), Times.Once());
        }

        [Test]
        public void ensure_version_repository_is_initialised_when_running_an_up_migration()
        {
            var migrationInfo = new MigrationInfo(direction: MigrationDirection.Up, scriptFolder: Some.String(), targetSchema: "testSchema",
                targetTablespace: "testTablespace", targetEnvironment: Some.ListOf(Some.String()), targetVersion: null);

            _migrator.Migrate(Some.ConnectionInfo(), migrationInfo);

            _mockVersionRepository.Verify(m => m.InitialiseVersioningTable("testSchema", "testTablespace"));
        }

        [Test]
        public void ensure_applied_scripts_repository_is_initialised_when_running_an_up_migration()
        {
            var migrationInfo = new MigrationInfo(direction: MigrationDirection.Up, scriptFolder: Some.String(), targetSchema: "testSchema",
                targetTablespace: "testTablespace", targetEnvironment: Some.ListOf(Some.String()), targetVersion: null);

            _migrator.Migrate(Some.ConnectionInfo(), migrationInfo);

            _mockAppliedScriptRespository.Verify(m => m.InitialiseAppliedScriptsTable("testSchema", "testTablespace"));
        }

        [Test]
        public void when_there_are_applied_scripts_stored_in_the_target_database_then_those_can_be_retrieved_for_down_migrations()
        {
            const string downScriptPath = "RB_20150406_scriptpath";
            _mockScriptScannerFactory.GetMock(ScriptFolders.Down).Setup(m => m.Scan(It.IsAny<string>(), It.IsAny<string>())).Returns(new[] { new Script(downScriptPath, 1) });

            _migrator.Migrate(Some.ConnectionInfo(), new MigrationInfo(direction: MigrationDirection.Down, scriptFolder: Some.String(), targetSchema: "testSchema",
                targetTablespace: "testTablespace", targetEnvironment: Some.ListOf(Some.String()), targetVersion: null, useStored: true));

            _mockAppliedScriptRespository.Verify(m => m.GetAppliedScripts("testSchema"), Times.Once);
        }

        [Test]
        public void when_target_environment_is_not_provided_then_there_are_no_errors()
        {
            var migrationInfo = new MigrationInfo(direction: MigrationDirection.Up, scriptFolder: Some.String(), targetSchema: "testSchema",
                targetTablespace: "testTablespace", targetEnvironment: null, targetVersion: null);

            Assert.DoesNotThrow(() => _migrator.Migrate(Some.ConnectionInfo(), migrationInfo));
        }

    }
}
