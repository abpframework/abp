using System.Diagnostics;

namespace Volo.Abp.Cli.Utils;

public interface ICmdHelper
{
    void Open(string pathOrUrl);

    void Run(string file, string arguments);

    string GetArguments(string command, int? delaySeconds = null);

    string GetFileName();

    void RunCmd(string command, string workingDirectory = null);

    Process RunCmdAndGetProcess(string command, string workingDirectory = null);

    void RunCmd(string command, out int exitCode, string workingDirectory = null);

    string RunCmdAndGetOutput(string command, string workingDirectory = null);

    string RunCmdAndGetOutput(string command, out bool isExitCodeSuccessful, string workingDirectory = null);

    string RunCmdAndGetOutput(string command, out int exitCode, string workingDirectory = null);

    void RunCmdAndExit(string command, string workingDirectory = null, int? delaySeconds = null);

}
