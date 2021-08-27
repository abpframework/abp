namespace Volo.Abp.Studio.Helpers
{
    public interface ICmdHelper
    {
        void RunCmd(string command, string workingDirectory = null);

        string RunCmdAndGetOutput(string command, string workingDirectory = null);
    }
}