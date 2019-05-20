using System.Diagnostics;
using System.Runtime.InteropServices;

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
            var procStartInfo = new ProcessStartInfo(GetFileName(), GetArguments(command));
            Process.Start(procStartInfo).WaitForExit();
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
                //TODO: Test this. it should work for both operaion systems.
                return "/bin/bash";
            }

            //Windows default.
            return "cmd.exe"; 
        }
    }
}
