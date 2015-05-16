namespace omt
{
    public interface IMigrator
    {
        void Migrate(DatabaseConnectionInfo databaseConnectionInfo, MigrationInfo migrationInfo);
    }
}
