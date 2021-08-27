using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Studio.Helpers
{
    //todo: move this to volo.abp.cli.core!
    public  class CmdHelper :  ICmdHelper, ITransientDependency
    {
        public void RunCmd(string command, string workingDirectory = null)
        {
            var procStartInfo = new ProcessStartInfo(
                GetFileName(),
                GetArguments(command)
            );

            if (!string.IsNullOrEmpty(workingDirectory))
            {
                procStartInfo.WorkingDirectory = workingDirectory;
            }

            using (var process = Process.Start(procStartInfo))
            {
                process?.WaitForExit();
            }
        }

        public string RunCmdAndGetOutput(string command, string workingDirectory = null)
        {
            string output;

            using (var process = new Process())
            {
                process.StartInfo = new ProcessStartInfo(GetFileName())
                {
                    Arguments = GetArguments(command),
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                };

                if (!string.IsNullOrEmpty(workingDirectory))
                {
                    process.StartInfo.WorkingDirectory = workingDirectory;
                }

                process.Start();

                using (var standardOutput = process.StandardOutput)
                {
                    using (var standardError = process.StandardError)
                    {
                        output = standardOutput.ReadToEnd();
                        output += standardError.ReadToEnd();
                    }
                }

                process.WaitForExit();
            }

            return output.Trim();
        }

        private string GetArguments(string command)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX) || RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return "-c \"" + command + "\"";
            }

            //Windows default.
            return "/C \"" + command + "\"";
        }

        private string GetFileName()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                //Windows
                return "cmd.exe";
            }

            //Linux or OSX
            if (File.Exists("/bin/bash"))
            {
                return "/bin/bash";
            }

            if (File.Exists("/bin/sh"))
            {
                return "/bin/sh"; //some Linux distributions like Alpine doesn't have bash
            }

            throw new AbpException($"Cannot determine shell command for this OS! " +
                                   $"Running on OS: {System.Runtime.InteropServices.RuntimeInformation.OSDescription} | " +
                                   $"OS Architecture: {System.Runtime.InteropServices.RuntimeInformation.OSArchitecture} | " +
                                   $"Framework: {System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription} | " +
                                   $"Process Architecture{System.Runtime.InteropServices.RuntimeInformation.ProcessArchitecture}");
        }
    }
}
