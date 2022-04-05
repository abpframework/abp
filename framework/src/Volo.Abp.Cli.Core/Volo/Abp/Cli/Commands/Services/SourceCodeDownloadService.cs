using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.Cli.Args;
using Volo.Abp.Cli.ProjectBuilding;
using Volo.Abp.Cli.ProjectBuilding.Building;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.Commands.Services;

public class SourceCodeDownloadService : ITransientDependency
{
    public ModuleProjectBuilder ModuleProjectBuilder { get; }
    public NugetPackageProjectBuilder NugetPackageProjectBuilder { get; }
    public NpmPackageProjectBuilder NpmPackageProjectBuilder { get; }
    public ILogger<SourceCodeDownloadService> Logger { get; set; }

    public SourceCodeDownloadService(ModuleProjectBuilder moduleProjectBuilder,
        NugetPackageProjectBuilder nugetPackageProjectBuilder,
        NpmPackageProjectBuilder npmPackageProjectBuilder)
    {
        ModuleProjectBuilder = moduleProjectBuilder;
        NugetPackageProjectBuilder = nugetPackageProjectBuilder;
        NpmPackageProjectBuilder = npmPackageProjectBuilder;
        Logger = NullLogger<SourceCodeDownloadService>.Instance;
    }

    public async Task DownloadModuleAsync(string moduleName, string outputFolder, string version, string gitHubAbpLocalRepositoryPath, string gitHubVoloLocalRepositoryPath, AbpCommandLineOptions options)
    {
        Logger.LogInformation("Downloading source code of " + moduleName);
        Logger.LogInformation("Version: " + (version ?? "Latest"));
        Logger.LogInformation("Output folder: " + outputFolder);

        var result = await ModuleProjectBuilder.BuildAsync(
            new ProjectBuildArgs(
                SolutionName.Parse(moduleName),
                moduleName,
                version,
                outputFolder,
                DatabaseProvider.NotSpecified,
                DatabaseManagementSystem.NotSpecified,
                UiFramework.NotSpecified,
                null,
                false,
                gitHubAbpLocalRepositoryPath,
                gitHubVoloLocalRepositoryPath,
                null,
                options
            )
        );

        using (var templateFileStream = new MemoryStream(result.ZipContent))
        {
            using (var zipArchive = new ZipArchive(templateFileStream, ZipArchiveMode.Read))
            {
                foreach (var zipEntry in zipArchive.Entries)
                {
                    if (IsAngularTestFile(zipEntry.FullName))
                    {
                        continue;
                    }

                    var fullZipToPath = Path.Combine(outputFolder, zipEntry.FullName);
                    var directoryName = Path.GetDirectoryName(fullZipToPath);

                    if (!string.IsNullOrEmpty(directoryName))
                    {
                        Directory.CreateDirectory(directoryName);
                    }

                    var fileName = Path.GetFileName(fullZipToPath);
                    if (fileName.Length == 0)
                    {
                        continue;
                    }

                    using (var entryStream = zipEntry.Open())
                    using (var fileStream = File.Create(fullZipToPath))
                    {
                        await entryStream.CopyToAsync(fileStream);
                    }
                }
            }
        }

        Logger.LogInformation($"'{moduleName}' has been successfully downloaded to '{outputFolder}'");
    }

    public async Task DownloadNugetPackageAsync(string packageName, string outputFolder, string version)
    {
        Logger.LogInformation("Downloading source code of " + packageName);
        Logger.LogInformation("Version: " + (version ?? "Latest"));
        Logger.LogInformation("Output folder: " + outputFolder);

        var result = await NugetPackageProjectBuilder.BuildAsync(
            new ProjectBuildArgs(
                SolutionName.Parse(packageName),
                packageName,
                version,
                outputFolder
            )
        );

        using (var templateFileStream = new MemoryStream(result.ZipContent))
        {
            using (var zipArchive = new ZipArchive(templateFileStream, ZipArchiveMode.Read))
            {
                foreach (var zipEntry in zipArchive.Entries)
                {
                    var fullZipToPath = Path.Combine(outputFolder, zipEntry.FullName);
                    var directoryName = Path.GetDirectoryName(fullZipToPath);

                    if (!string.IsNullOrEmpty(directoryName))
                    {
                        Directory.CreateDirectory(directoryName);
                    }

                    var fileName = Path.GetFileName(fullZipToPath);
                    if (fileName.Length == 0)
                    {
                        continue;
                    }

                    using (var entryStream = zipEntry.Open())
                    using (var fileStream = File.Create(fullZipToPath))
                    {
                        await entryStream.CopyToAsync(fileStream);
                    }
                }
            }
        }

        Logger.LogInformation($"'{packageName}' has been successfully downloaded to '{outputFolder}'");
    }

    public async Task DownloadNpmPackageAsync(string packageName, string outputFolder, string version)
    {
        Logger.LogInformation("Downloading source code of " + packageName);
        Logger.LogInformation("Version: " + (version ?? "Latest"));
        Logger.LogInformation("Output folder: " + outputFolder);

        var result = await NpmPackageProjectBuilder.BuildAsync(
            new ProjectBuildArgs(
                SolutionName.Parse(packageName),
                packageName,
                version,
                outputFolder
            )
        );

        using (var templateFileStream = new MemoryStream(result.ZipContent))
        {
            using (var zipArchive = new ZipArchive(templateFileStream, ZipArchiveMode.Read))
            {
                foreach (var zipEntry in zipArchive.Entries)
                {
                    var fullZipToPath = Path.Combine(outputFolder, zipEntry.FullName);
                    var directoryName = Path.GetDirectoryName(fullZipToPath);

                    if (!string.IsNullOrEmpty(directoryName))
                    {
                        Directory.CreateDirectory(directoryName);
                    }

                    var fileName = Path.GetFileName(fullZipToPath);
                    if (fileName.Length == 0)
                    {
                        continue;
                    }

                    using (var entryStream = zipEntry.Open())
                    using (var fileStream = File.Create(fullZipToPath))
                    {
                        await entryStream.CopyToAsync(fileStream);
                    }
                }
            }
        }

        Logger.LogInformation($"'{packageName}' has been successfully downloaded to '{outputFolder}'");
    }

    private bool IsAngularTestFile(string zipEntryName)
    {
        if (string.IsNullOrEmpty(zipEntryName))
        {
            return false;
        }

        if (zipEntryName.StartsWith("angular/") && !zipEntryName.StartsWith("angular/projects"))
        {
            return true;
        }

        return false;
    }
}
