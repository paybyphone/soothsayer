namespace omt.Scanners
{
    public static class FilePattern
    {
        public static readonly string NoEnvironment = @"^[\w\-]+\.sql$";

        public static string ForEnvironment(string environment)
        {
            return @"^[\w\-]+\." + environment + @"\.sql$";
        }
    }
}