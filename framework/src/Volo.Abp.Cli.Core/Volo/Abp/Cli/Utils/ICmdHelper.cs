using System.Diagnostics;

namespace Volo.Abp.Cli.Utils;

public interface ICmdHelper
{
    void Open(string pathOrUrl);

    void Run(string file, string arguments, bool isAdministrative = false);

    string GetArguments(string command, int? delaySeconds = null);

    string GetFileName();

    void RunCmd(string command, string workingDirectory = null, bool isAdministrative = false);

    Process RunCmdAndGetProcess(string command, string workingDirectory = null, bool isAdministrative = false);

    void RunCmd(string command, out int exitCode, string workingDirectory = null, bool isAdministrative = false);

    string RunCmdAndGetOutput(string command, string workingDirectory = null, bool isAdministrative = false);

    string RunCmdAndGetOutput(string command, out bool isExitCodeSuccessful, string workingDirectory = null, bool isAdministrative = false);

    string RunCmdAndGetOutput(string command, out int exitCode, string workingDirectory = null, bool isAdministrative = false);

    void RunCmdAndExit(string command, string workingDirectory = null, int? delaySeconds = null, bool isAdministrative = false);

}
