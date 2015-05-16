using System;
using System.Collections.Specialized;
using System.Configuration;

namespace omt.Infrastructure
{
    public class ConfigSection
    {
        private readonly string _configSectionName;
        private readonly NameValueCollection _configSection;

        public ConfigSection(string configSectionName)
        {
            _configSectionName = configSectionName;
            _configSection = ConfigurationManager.GetSection(_configSectionName) as NameValueCollection;
        }

        public T GetConfigurationValue<T>(string key)
        {
            if (_configSection == null)
            {
                throw new MissingConfigurationSectionException(_configSectionName);
            }

            var value = _configSection[key];

            if (value == null)
            {
                throw new MissingConfigurationValueException(key, _configSectionName);
            }

            return (T)Convert.ChangeType(value, typeof(T));
        }

    }
}