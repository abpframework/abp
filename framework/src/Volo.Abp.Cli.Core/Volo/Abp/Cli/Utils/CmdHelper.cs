using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Volo.Abp.Cli.Utils
{
    public static class CmdHelper
    {
        public static int SuccessfulExitCode = 0;

        public static void Run(string file, string arguments)
        {
            var procStartInfo = new ProcessStartInfo(file, arguments);
            Process.Start(procStartInfo)?.WaitForExit();
        }

        public static int RunCmd(string command)
        {
            var procStartInfo = new ProcessStartInfo(
                GetFileName(),
                GetArguments(command)
            );

            using (var process = Process.Start(procStartInfo))
            {
                process?.WaitForExit();
                return process?.ExitCode ?? 0;
            }
        }

        public static string RunCmdAndGetOutput(string command)
        {
            return RunCmdAndGetOutput(command, out int _);
        }

        public static string RunCmdAndGetOutput(string command, out bool isExitCodeSuccessful)
        {
            var output = RunCmdAndGetOutput(command, out int exitCode);
            isExitCodeSuccessful = exitCode == SuccessfulExitCode;
            return output;
        }

        public static string RunCmdAndGetOutput(string command, out int exitCode)
        {
            string output;

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

                using (var standardOutput = process.StandardOutput)
                {
                    using (var standardError = process.StandardError)
                    {
                        output = standardOutput.ReadToEnd();
                        output += standardError.ReadToEnd();
                    }
                }

                process.WaitForExit();

                exitCode = process.ExitCode;
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
