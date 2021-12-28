using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.Utils;

public class CmdHelper : ICmdHelper, ITransientDependency
{
    private const int SuccessfulExitCode = 0;

    public void OpenWebPage(string url)
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


    public void Run(string file, string arguments)
    {
        var procStartInfo = new ProcessStartInfo(file, arguments);
        Process.Start(procStartInfo)?.WaitForExit();
    }

    public void RunCmd(string command, string workingDirectory = null)
    {
        RunCmd(command, out _, workingDirectory);
    }

    public void RunCmd(string command, out int exitCode, string workingDirectory = null)
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

            exitCode = process.ExitCode;
        }
    }

    public string RunCmdAndGetOutput(string command, string workingDirectory = null)
    {
        return RunCmdAndGetOutput(command, out int _, workingDirectory);
    }

    public string RunCmdAndGetOutput(string command, out bool isExitCodeSuccessful, string workingDirectory = null)
    {
        var output = RunCmdAndGetOutput(command, out int exitCode, workingDirectory);
        isExitCodeSuccessful = exitCode == SuccessfulExitCode;
        return output;
    }

    public string RunCmdAndGetOutput(string command, out int exitCode, string workingDirectory = null)
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

            exitCode = process.ExitCode;
        }

        return output.Trim();
    }

    public string GetArguments(string command)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX) || RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            return "-c \"" + command + "\"";
        }

        //Windows default.
        return "/C \"" + command + "\"";
    }

    public string GetFileName()
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
