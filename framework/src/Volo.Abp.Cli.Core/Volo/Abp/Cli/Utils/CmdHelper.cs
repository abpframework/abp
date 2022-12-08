﻿using System;
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

    public void Run(string file, string arguments)
    {
        var procStartInfo = new ProcessStartInfo(file, arguments);

        if (CliOptions.AlwaysHideExternalCommandOutput)
        {
            HideNewCommandWindow(procStartInfo);
        }
        
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

    public Process RunCmdAndGetProcess(string command, string workingDirectory = null)
    {
        var procStartInfo = new ProcessStartInfo(
            GetFileName(),
            GetArguments(command)
        );

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
                WindowStyle = ProcessWindowStyle.Hidden,
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

    public void RunCmdAndExit(string command, string workingDirectory = null, int? delaySeconds = null)
    {
        var procStartInfo = new ProcessStartInfo(
            GetFileName(),
            GetArguments(command, delaySeconds)
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
            return delaySeconds == null ? "-c \"" + command + "\"" : "-c \"" + $"sleep {delaySeconds}s > /dev/null && " + command + "\"";
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
