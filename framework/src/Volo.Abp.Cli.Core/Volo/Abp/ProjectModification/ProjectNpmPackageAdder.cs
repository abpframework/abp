using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.ProjectModification
{
    public class ProjectNpmPackageAdder : ITransientDependency
    {
        public ILogger<ProjectNpmPackageAdder> Logger { get; set; }

        public ProjectNpmPackageAdder()
        {
            Logger = NullLogger<ProjectNpmPackageAdder>.Instance;
        }

        public Task AddAsync(string directory, NpmPackageInfo npmPackage)
        {
            var packageJsonFilePath = Path.Combine(directory, "package.json");
            if (!File.Exists(packageJsonFilePath))
            {
                return Task.CompletedTask;
            }

            Logger.LogInformation($"Installing '{npmPackage.Name}' package to the project '{packageJsonFilePath}'...");

            Logger.LogInformation("yarn add " + npmPackage.Name + "...");
            var procStartInfo = new ProcessStartInfo("cmd.exe", "/C yarn add " + npmPackage.Name);
            procStartInfo.WindowStyle = ProcessWindowStyle.Normal;
            Process.Start(procStartInfo).WaitForExit();

            Logger.LogInformation("gulp...");
            procStartInfo = new ProcessStartInfo("cmd.exe", "/C gulp");
            procStartInfo.WindowStyle = ProcessWindowStyle.Normal;
            Process.Start(procStartInfo).WaitForExit();

            return Task.CompletedTask;
        }
    }
}
