using System.IO;

namespace Volo.Abp.Cli.Utils
{
    public class GlobalToolHelper
    {
        /// <summary>
        /// Checks whether the tool is installed or not.
        /// </summary>
        /// <param name="toolCommandName">Eg: For AbpSuite tool it's "abp-suite", for ABP CLI tool it's "abp"</param>
        public static bool IsGlobalToolInstalled(string toolCommandName)
        {
            if (PlatformHelper.GetPlatform() == RuntimePlatform.LinuxOrMacOs)
            {
                return File.Exists("%HOME%/.dotnet/tools/" + toolCommandName);
            }

            return File.Exists(@"%USERPROFILE%\.dotnet\tools\" + toolCommandName + ".exe");
        }
    }
}