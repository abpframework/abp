using System;
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
            string suitePath;

            if (PlatformHelper.GetPlatform() == RuntimePlatform.LinuxOrMacOs)
            {
                suitePath = Environment
                    .ExpandEnvironmentVariables(
                        Path.Combine("%HOME%", ".dotnet", "tools", toolCommandName)
                    );
            }
            else
            {
                suitePath = Environment
                    .ExpandEnvironmentVariables(
                        Path.Combine(@"%USERPROFILE%", ".dotnet", "tools", toolCommandName + ".exe")
                    );
            }

            return File.Exists(suitePath);
        }
    }
}