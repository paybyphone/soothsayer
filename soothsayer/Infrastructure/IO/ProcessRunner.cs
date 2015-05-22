using System;
using System.Diagnostics;

namespace soothsayer.Infrastructure.IO
{
    public class ProcessRunner : IProcessRunner
    {
        public int Execute(string processFullPath, string arguments)
        {
            var sqlPlusProcessInfo = new ProcessStartInfo(processFullPath, arguments) { UseShellExecute = false, RedirectStandardOutput = true };
            //UTF-8 support
            sqlPlusProcessInfo.EnvironmentVariables.Add("NLS_LANG", "American_America.AL32UTF8");

            var process = new Process { StartInfo = sqlPlusProcessInfo };
            process.OutputDataReceived += ShowSqlPlusProcessOutput;
            process.Start();
            process.BeginOutputReadLine();

            process.WaitForExit();

            var exitCode = process.ExitCode;

            return exitCode;
        }

        private void ShowSqlPlusProcessOutput(object sender, DataReceivedEventArgs e)
        {
            Console.WriteLine(e.Data);
        }
    }
}
