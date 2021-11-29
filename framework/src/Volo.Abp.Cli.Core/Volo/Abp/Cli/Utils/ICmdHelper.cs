namespace Volo.Abp.Cli.Utils
{
    public interface ICmdHelper
    {
        void OpenWebPage(string url);

        void Run(string file, string arguments);

        string GetArguments(string command);

        string GetFileName();

        void RunCmd(string command, string workingDirectory = null);

        void RunCmd(string command, out int exitCode, string workingDirectory = null);

        string RunCmdAndGetOutput(string command, string workingDirectory = null);


        string RunCmdAndGetOutput(string command, out bool isExitCodeSuccessful, string workingDirectory = null);

        string RunCmdAndGetOutput(string command, out int exitCode, string workingDirectory = null);
    }
}
