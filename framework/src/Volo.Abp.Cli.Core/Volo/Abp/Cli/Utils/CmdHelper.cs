using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.Utils;

public class CmdHelper : ICmdHelper, ITransientDependency
{
    private const int SuccessfulExitCode = 0;

    protected AbpCliOptions CliOptions { get; }

    public CmdHelper(IOptionsSnapshot<AbpCliOptions> cliOptions)
    {
        CliOptions = cliOptions.Value;
    }

    public void Open(string pathOrUrl)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            pathOrUrl = pathOrUrl.Replace("&", "^&");
            Process.Start(new ProcessStartInfo("cmd", $"/c start {pathOrUrl}") { CreateNoWindow = true });
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            Process.Start("xdg-open", pathOrUrl);
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            Process.Start("open", pathOrUrl);
        }
    }

    public void Run(string file, string arguments, bool isAdministrative = false)
    {
        var procStartInfo = GetProcessStartInfo(file, arguments, isAdministrative);

        if (CliOptions.AlwaysHideExternalCommandOutput)
        {
            HideNewCommandWindow(procStartInfo);
        }

        Process.Start(procStartInfo)?.WaitForExit();
    }

    public void RunCmd(string command, string workingDirectory = null, bool isAdministrative = false)
    {
        RunCmd(command, out _, workingDirectory, isAdministrative);
    }

    public void RunCmd(string command, out int exitCode, string workingDirectory = null, bool isAdministrative = false)
    {
        var procStartInfo = GetProcessStartInfo(GetFileName(), GetArguments(command), isAdministrative);

        if (CliOptions.AlwaysHideExternalCommandOutput)
        {
            HideNewCommandWindow(procStartInfo);
        }

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

    private ProcessStartInfo GetProcessStartInfo(string fileName, string arguments, bool isAdministrative = false)
    {
        if (isAdministrative)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return new ProcessStartInfo(fileName, arguments)
                {
                    Verb = "runas",
                    UseShellExecute = true
                };
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                return new ProcessStartInfo("osascript", $"-e \"do shell script \\\"{fileName} {arguments}\\\" with administrator privileges\"");
            }
            else
            {
                return new ProcessStartInfo("sudo", $"{fileName} {arguments}");
            }
        }

        return new ProcessStartInfo(fileName, arguments);
    }

    public Process RunCmdAndGetProcess(string command, string workingDirectory = null, bool isAdministrative = false)
    {
        var procStartInfo = GetProcessStartInfo(GetFileName(), GetArguments(command), isAdministrative);

        if (CliOptions.AlwaysHideExternalCommandOutput)
        {
            HideNewCommandWindow(procStartInfo);
        }

        if (!string.IsNullOrEmpty(workingDirectory))
        {
            procStartInfo.WorkingDirectory = workingDirectory;
        }

        return Process.Start(procStartInfo);
    }

    public string RunCmdAndGetOutput(string command, string workingDirectory = null, bool isAdministrative = false)
    {
        return RunCmdAndGetOutput(command, out int _, workingDirectory, isAdministrative);
    }

    public string RunCmdAndGetOutput(string command, out bool isExitCodeSuccessful, string workingDirectory = null, bool isAdministrative = false)
    {
        var output = RunCmdAndGetOutput(command, out int exitCode, workingDirectory, isAdministrative);
        isExitCodeSuccessful = exitCode == SuccessfulExitCode;
        return output;
    }

    public string RunCmdAndGetOutput(string command, out int exitCode, string workingDirectory = null, bool isAdministrative = false)
    {
        string output;

        using (var process = new Process())
        {
            process.StartInfo = GetProcessStartInfo(GetFileName(), GetArguments(command), isAdministrative);

            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;

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

    public void RunCmdAndExit(string command, string workingDirectory = null, int? delaySeconds = null, bool isAdministrative = false)
    {
        var procStartInfo = GetProcessStartInfo(
            GetFileName(),
            GetArguments(command, delaySeconds),
            isAdministrative
        );

        if (!string.IsNullOrEmpty(workingDirectory))
        {
            procStartInfo.WorkingDirectory = workingDirectory;
        }

        if (CliOptions.AlwaysHideExternalCommandOutput)
        {
            HideNewCommandWindow(procStartInfo);
        }

        Process.Start(procStartInfo);
        Environment.Exit(0);
    }

    public string GetArguments(string command, int? delaySeconds = null)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX) || RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            return delaySeconds == null ? "-c \"" + command + "\"" : "-c \"" + $"sleep {delaySeconds} > /dev/null && " + command + "\"";
        }

        //Windows default.
        return delaySeconds == null ? "/C \"" + command + "\"" : "/C \"" + $"timeout /nobreak /t {delaySeconds} >null && " + command + "\"";
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

    private void HideNewCommandWindow(ProcessStartInfo procStartInfo)
    {
        procStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
        procStartInfo.CreateNoWindow = true;
    }
}
