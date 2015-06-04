﻿using System.Data;
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
        private Mock<IDatabaseMetadataProvider> _mockDatabaseMetadataProvider;
        private Mock<IDatabaseMetadataProviderFactory> _mockDatabaseMetadataProviderFactory;
        private MockScriptScannerFactory _mockScriptScannerFactory;
        private Mock<IScriptRunner> _mockScriptRunner;
        private Mock<IScriptRunnerFactory> _mockScriptRunnerFactory;
        private Mock<IColorConsole> _mockOutput;

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

            _mockDatabaseMetadataProvider = new Mock<IDatabaseMetadataProvider>();
            _mockDatabaseMetadataProviderFactory = new Mock<IDatabaseMetadataProviderFactory>();
            _mockDatabaseMetadataProviderFactory.Setup(m => m.Create(It.IsAny<IDbConnection>())).Returns(_mockDatabaseMetadataProvider.Object);

            _mockScriptScannerFactory = new MockScriptScannerFactory();

            _mockScriptRunner = new Mock<IScriptRunner>();
            _mockScriptRunnerFactory = new Mock<IScriptRunnerFactory>();
            _mockScriptRunnerFactory.Setup(m => m.Create(It.IsAny<DatabaseConnectionInfo>())).Returns(_mockScriptRunner.Object);
        }

        [Test]
        public void all_supported_script_folders_are_scanned()
        {
            var migrator = new OracleMigrator(_mockConnectionFactory.Object, _mockVersionRepositoryFactory.Object, _mockDatabaseMetadataProviderFactory.Object, _mockScriptScannerFactory, _mockScriptRunnerFactory.Object);

            DatabaseConnectionInfo databaseConnectionInfo = new DatabaseConnectionInfo(Some.String(), Some.String(), Some.String());
            MigrationInfo migrationInfo = new MigrationInfo(Some.Value<MigrationDirection>(), Some.String(), Some.String(), Some.String(), Some.String(), null);

            migrator.Migrate(databaseConnectionInfo, migrationInfo);

            foreach (var mockScriptScanner in _mockScriptScannerFactory.GetMocks())
            {
                mockScriptScanner.Verify(m => m.Scan(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            }
        }

        [Test]
        public void when_an_up_script_is_missing_a_corresponding_down_script_then_a_warning_is_displayed()
        {
            var migrator = new OracleMigrator(_mockConnectionFactory.Object, _mockVersionRepositoryFactory.Object, _mockDatabaseMetadataProviderFactory.Object, _mockScriptScannerFactory, _mockScriptRunnerFactory.Object);

            DatabaseConnectionInfo databaseConnectionInfo = new DatabaseConnectionInfo(Some.String(), Some.String(), Some.String());
            MigrationInfo migrationInfo = new MigrationInfo(Some.Value<MigrationDirection>(), Some.String(), Some.String(), Some.String(), Some.String(), null);

            const string upScriptPath = "20150406_scriptpath";
            _mockScriptScannerFactory.GetMock(ScriptFolders.Up).Setup(m => m.Scan(It.IsAny<string>(), It.IsAny<string>())).Returns(new[] { new Script(upScriptPath, 1) });

            migrator.Migrate(databaseConnectionInfo, migrationInfo);

            _mockOutput.Verify(m => m.WriteLine("The following 'up' scripts do not have a corresponding 'down' script, any rollback may not work as expected:".Yellow()));
            _mockOutput.Verify(m => m.WriteLine("    {0}".FormatWith(upScriptPath).DarkGray()), Times.Once());
        }
    }
}