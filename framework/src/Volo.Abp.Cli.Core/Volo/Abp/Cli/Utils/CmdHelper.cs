using System.Diagnostics;

namespace Volo.Abp.Cli.Utils
{
    public static class CmdHelper
    {
        public static void Run(string file, string arguments)
        {
            var procStartInfo = new ProcessStartInfo(file, arguments);
            Process.Start(procStartInfo).WaitForExit();
        }

        public static void RunCmd(string command)
        {
            var procStartInfo = new ProcessStartInfo("cmd.exe", "/C " + command);
            Process.Start(procStartInfo).WaitForExit();
        }
    }
}
