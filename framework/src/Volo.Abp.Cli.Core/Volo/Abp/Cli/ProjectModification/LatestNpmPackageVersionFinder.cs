using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Volo.Abp.Cli.Utils;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.ProjectModification
{
    public class LatestNpmPackageVersionFinder : ITransientDependency
    {
        public string Find(string packageName)
        {
            var output = "";

            using (var process = new Process())
            {
                process.StartInfo = new ProcessStartInfo(CmdHelper.GetFileName())
                {
                    Arguments = CmdHelper.GetArguments($"npm show {packageName} version"),
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                };
                process.Start();

                using (var stdOut = process.StandardOutput)
                {
                    using (var stdErr = process.StandardError)
                    {
                        output = stdOut.ReadToEnd();
                        output += stdErr.ReadToEnd();
                    }
                }
            }

            return output.Trim();
        }
    }
}
