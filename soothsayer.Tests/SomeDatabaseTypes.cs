namespace soothsayer.Tests
{
    public sealed partial class Some
    {
        public static DatabaseConnectionInfo ConnectionInfo()
        {
            return new DatabaseConnectionInfo(String(), String(), String());
        }

        public static MigrationInfo MigrationInfo(MigrationDirection? direction = null)
        {
            return new MigrationInfo(direction ?? Value<MigrationDirection>(), String(), String(), String(), ListOf(String()), null);
        }
    }
}
