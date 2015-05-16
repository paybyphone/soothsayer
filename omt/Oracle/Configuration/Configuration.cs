using omt.Infrastructure;

namespace omt.Oracle.Configuration
{
    public class SqlPlusConfiguration : ISqlPlusConfiguration
    {
        private readonly ConfigSection _configuration;

        public SqlPlusConfiguration()
        {
            const string sectionName = "sqlPlus";
            _configuration = new ConfigSection(sectionName);
        }

        public string RunnerPath
        {
            get { return _configuration.GetConfigurationValue<string>(PropertyName.For(() => RunnerPath)); }
        }
    }
}