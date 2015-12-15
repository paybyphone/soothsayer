using System;
using System.IO;

namespace soothsayer.Scripts
{
    public class StoredScript : IScript, IDisposable
    {
        private readonly long _version;
        private readonly string _scriptName;
        private readonly string _temporaryScriptPath;

        public StoredScript(long version, string scriptName, string scriptContents)
        {
            _version = version;
            _scriptName = scriptName;
            _temporaryScriptPath = System.IO.Path.GetTempFileName();

            using (var fileStream = File.OpenWrite(_temporaryScriptPath))
            {
                using (var tempFile = new StreamWriter(fileStream, UTF8.WithoutByteOrderMark))
                {
                    tempFile.WriteLine(scriptContents);
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
                File.Delete(_temporaryScriptPath);
            }
        }

        public string Path { get { return _temporaryScriptPath; } }

        public string Name { get { return _scriptName; } }

        public long Version { get { return _version; } }
    }
}
