namespace omt
{
    public class DatabaseConnectionInfo
    {
        public DatabaseConnectionInfo(string connectionString, string username, string password)
        {
            ConnectionString = connectionString;
            Username = username;
            Password = password;
        }

        public string ConnectionString { get; private set; }
        public string Username { get; private set; }
        public string Password { get; private set; }
    }
}
