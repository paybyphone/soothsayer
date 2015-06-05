using System;

namespace soothsayer.Infrastructure.Configuration
{
    [Serializable]
    internal class MissingConfigurationValueException : Exception
    {
        public MissingConfigurationValueException(string key, string configSectionName)
            : base("Configuration value for '{0}' could not be found in configuration section '{1}'".FormatInvariant(key, configSectionName))
        {
        }
    }
}
