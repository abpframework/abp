using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.DependencyInjection;
using Volo.Abp.IO;

namespace Volo.Abp.Cli.ProjectModification
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

            using (DirectoryHelper.ChangeCurrentDirectory(directory))
            {
                Logger.LogInformation("yarn add " + npmPackage.Name + "... " + Directory.GetCurrentDirectory());
                var procStartInfo = new ProcessStartInfo("cmd.exe", "/C yarn add " + npmPackage.Name);
                Process.Start(procStartInfo).WaitForExit();

                Logger.LogInformation("gulp... " + Directory.GetCurrentDirectory());
                procStartInfo = new ProcessStartInfo("cmd.exe", "/C gulp");
                Process.Start(procStartInfo).WaitForExit();
            }

            return Task.CompletedTask;
        }
    }
}
