namespace soothsayer.Tests
{
    public sealed partial class Some
    {
        public static DatabaseConnectionInfo ConnectionInfo()
        {
            return new DatabaseConnectionInfo(String(), String(), String());
        }

        public static MigrationInfo MigrationInfo()
        {
            return new MigrationInfo(Value<MigrationDirection>(), String(), String(), String(), ListOf(String()), null);
        }
    }
}
