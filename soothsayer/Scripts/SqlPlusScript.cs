using System;
using System.IO;

namespace soothsayer.Scripts
{
    public class SqlPlusScript : IScript, IDisposable
    {
        private readonly string _wrappedScriptPath;
        private readonly IScript _script;

        public SqlPlusScript(IScript script)
        {
            _script = script;

            _wrappedScriptPath = System.IO.Path.GetTempFileName();

            var reader = new ScriptReader();

            using (var fileStream = File.OpenWrite(_wrappedScriptPath))
            {
                using (var tempFile = new StreamWriter(fileStream, UTF8.WithoutByteOrderMark))
                {
                    tempFile.WriteLine("SET ECHO ON");
                    tempFile.WriteLine("WHENEVER SQLERROR EXIT SQL.SQLCODE");

                    foreach (var scriptLine in reader.GetContents(_script.Path))
                    {
                        tempFile.WriteLine(scriptLine);
                    }

                    tempFile.WriteLine("COMMIT;");
                    tempFile.WriteLine("EXIT");
                }
            }
        }
        
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                File.Delete(_wrappedScriptPath);
            }
        }

        public string Path { get { return _wrappedScriptPath; } }

        public string Name
        {
            get
            {
                return System.IO.Path.GetFileName(Path);
            }
        }

        public long Version { get { return _script.Version; } }
    }
}
