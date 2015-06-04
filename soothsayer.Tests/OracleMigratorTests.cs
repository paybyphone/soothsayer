using System.Data;
using Moq;
using NUnit.Framework;
using soothsayer.Scanners;

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
        private Mock<IScriptScanner> _mockScriptScanner;
        private Mock<IScriptScannerFactory> _mockScriptScannerFactory;

        [SetUp]
        public void SetUp()
        {
            _mockConnectionFactory = new Mock<IConnectionFactory>();

            _mockVersionRepository = new Mock<IVersionRespository>();
            _mockVersionRepositoryFactory = new Mock<IVersionRespositoryFactory>();
            _mockVersionRepositoryFactory.Setup(m => m.Create(It.IsAny<IDbConnection>())).Returns(_mockVersionRepository.Object);

            _mockDatabaseMetadataProvider = new Mock<IDatabaseMetadataProvider>();
            _mockDatabaseMetadataProviderFactory = new Mock<IDatabaseMetadataProviderFactory>();
            _mockDatabaseMetadataProviderFactory.Setup(m => m.Create(It.IsAny<IDbConnection>())).Returns(_mockDatabaseMetadataProvider.Object);

            _mockScriptScanner = new Mock<IScriptScanner>();
            _mockScriptScannerFactory = new Mock<IScriptScannerFactory>();
            _mockScriptScannerFactory.Setup(m => m.Create(It.IsAny<string>())).Returns(_mockScriptScanner.Object);
        }

        [Test]
        public void foo()
        {
            var migrator = new OracleMigrator(_mockConnectionFactory.Object, _mockVersionRepositoryFactory.Object, _mockDatabaseMetadataProviderFactory.Object, _mockScriptScannerFactory.Object);

            DatabaseConnectionInfo databaseConnectionInfo = new DatabaseConnectionInfo(Some.String(), Some.String(), Some.String());
            MigrationInfo migrationInfo = new MigrationInfo(Some.Value<MigrationDirection>(), Some.String(), Some.String(), Some.String(), Some.String(), null);

            migrator.Migrate(databaseConnectionInfo, migrationInfo);


        }
    }
}
