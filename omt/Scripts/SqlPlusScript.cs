using System;
using System.IO;
using System.Text;

namespace omt.Scripts
{
    public class SqlPlusScript : IScript, IDisposable
    {
        private readonly string _wrappedScriptPath;
        private readonly IScript _script;

        public SqlPlusScript(IScript script)
        {
            _script = script;

            _wrappedScriptPath = System.IO.Path.GetTempFileName();

            using (var fileStream = File.OpenWrite(_wrappedScriptPath))
            {
                var utfWithoutByteOrderMark = new UTF8Encoding(false);
                using (var tempFile = new StreamWriter(fileStream, utfWithoutByteOrderMark))
                {
                    tempFile.WriteLine("SET ECHO ON");
                    tempFile.WriteLine("WHENEVER SQLERROR EXIT SQL.SQLCODE");

                    foreach (var scriptLine in File.ReadAllLines(_script.Path))
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
