using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace Volo.Abp.Cli.Utils
{
    public static class CmdHelper
    {
        public static int SuccessfulExitCode = 0;

        public static void OpenWebPage(string url)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                url = url.Replace("&", "^&");
                Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                Process.Start("xdg-open", url);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                Process.Start("open", url);
            }
        }

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
                    Arguments = GetArguments(command),
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
