using omt.Scripts;

namespace omt
{
    public class DatabaseVersion
    {
        // ReSharper disable once UnusedMember.Local
        private DatabaseVersion()
        {
        }

        public DatabaseVersion(long version, string scriptName)
        {
            ScriptName = scriptName;
            Version = version;
        }

        public long Version { get; private set; }
        public string ScriptName { get; private set; }


        protected bool Equals(DatabaseVersion other)
        {
            return Version == other.Version && string.Equals(ScriptName, other.ScriptName);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj.GetType() != GetType())
            {
                return false;
            }
            return Equals((DatabaseVersion)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Version.GetHashCode() * 397) ^ (ScriptName != null ? ScriptName.GetHashCode() : 0);
            }
        }

    }

    public static class DatabaseVersionExtensions
    {
        public static DatabaseVersion AsDatabaseVersion(this IScript script)
        {
            return new DatabaseVersion(script.Version, script.Name);
        }
    }
}