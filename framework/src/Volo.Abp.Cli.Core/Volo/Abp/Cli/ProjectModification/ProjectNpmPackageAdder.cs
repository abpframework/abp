using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.Cli.Utils;
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

        public Task AddAsync(string directory, NpmPackageInfo npmPackage, bool skipGulpCommand = false)
        {
            var packageJsonFilePath = Path.Combine(directory, "package.json");
            if (!File.Exists(packageJsonFilePath) ||
                File.ReadAllText(packageJsonFilePath).Contains($"\"{npmPackage.Name}\""))
            {
                return Task.CompletedTask;
            }

            Logger.LogInformation($"Installing '{npmPackage.Name}' package to the project '{packageJsonFilePath}'...");



            using (DirectoryHelper.ChangeCurrentDirectory(directory))
            {
                Logger.LogInformation("yarn add " + npmPackage.Name);
                CmdHelper.RunCmd("yarn add " + npmPackage.Name);

                if (skipGulpCommand)
                {
                    return Task.CompletedTask;
                }

                Logger.LogInformation("gulp");
                CmdHelper.RunCmd("gulp");
            }

            return Task.CompletedTask;
        }
    }
}
