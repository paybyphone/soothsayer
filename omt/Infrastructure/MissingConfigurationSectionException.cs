using System;

namespace omt.Infrastructure
{
    [Serializable]
    internal class MissingConfigurationSectionException : Exception
    {
        public MissingConfigurationSectionException(string configSectionName)
            : base("Configuration section '{0}' could not be found".FormatInvariant(configSectionName))
        {
        }
    }
}