using System;

namespace soothsayer.Scripts
{
    public class Script : IScript
    {
        public string Path { get; private set; }

        public string Name
        {
            get
            {
                return System.IO.Path.GetFileName(Path);
            }
        }

        public long Version { get; private set; }

        public Script(string path, long version)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException("Script path cannot be null or empty", path);
            }

            Path = path;
            Version = version;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
