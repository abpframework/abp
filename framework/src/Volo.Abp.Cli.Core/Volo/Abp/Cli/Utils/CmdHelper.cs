using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Volo.Abp.Cli.Utils
{
    public static class CmdHelper
    {
        public static void Run(string file, string arguments)
        {
            var procStartInfo = new ProcessStartInfo(file, arguments);
            Process.Start(procStartInfo).WaitForExit();
        }

        public static int RunCmd(string command)
        {
            var procStartInfo = new ProcessStartInfo(GetFileName(), GetArguments(command));
            var process = Process.Start(procStartInfo);
            process?.WaitForExit();
            return process?.ExitCode ?? 0;
        }

        public static string RunCmdAndGetOutput(string command)
        {
            var output = "";

            using (var process = new Process())
            {
                process.StartInfo = new ProcessStartInfo(CmdHelper.GetFileName())
                {
                    Arguments = CmdHelper.GetArguments(command),
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                };
                process.Start();

                using (var stdOut = process.StandardOutput)
                {
                    using (var stdErr = process.StandardError)
                    {
                        output = stdOut.ReadToEnd();
                        output += stdErr.ReadToEnd();
                    }
                }
            }

            return output.Trim();
        }

        public static string GetArguments(string command)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX) || RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return "-c \"" + command + "\"";
            }

            //Windows default.
            return "/C \"" + command + "\"";
        }

        public static string GetFileName()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX) || RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                //TODO: Test this. it should work for both operation systems.
                return "/bin/bash";
            }

            //Windows default.
            return "cmd.exe";
        }
    }
}
