using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.Cli.Args;
using Volo.Abp.Cli.ProjectBuilding;
using Volo.Abp.Cli.ProjectBuilding.Building;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.Commands.Services
{
    public class SourceCodeDownloadService : ITransientDependency
    {
        public ModuleProjectBuilder ModuleProjectBuilder { get; }
        public ILogger<SourceCodeDownloadService> Logger { get; set; }

        public SourceCodeDownloadService(ModuleProjectBuilder moduleProjectBuilder)
        {
            ModuleProjectBuilder = moduleProjectBuilder;
            Logger = NullLogger<SourceCodeDownloadService>.Instance;
        }

        public async Task DownloadAsync(string moduleName, string outputFolder, string version, string gitHubLocalRepositoryPath, AbpCommandLineOptions options)
        {
            Logger.LogInformation("Downloading source code of " + moduleName);
            Logger.LogInformation("Version: " + version);
            Logger.LogInformation("Output folder: " + outputFolder);

            var result = await ModuleProjectBuilder.BuildAsync(
                new ProjectBuildArgs(
                    SolutionName.Parse(moduleName),
                    moduleName,
                    version,
                    DatabaseProvider.NotSpecified,
                    UiFramework.NotSpecified,
                    null,
                    gitHubLocalRepositoryPath,
                    null,
                    options
                )
            );

            using (var templateFileStream = new MemoryStream(result.ZipContent))
            {
                using (var zipInputStream = new ZipInputStream(templateFileStream))
                {
                    var zipEntry = zipInputStream.GetNextEntry();
                    while (zipEntry != null)
                    {
                        var fullZipToPath = Path.Combine(outputFolder, zipEntry.Name);
                        var directoryName = Path.GetDirectoryName(fullZipToPath);

                        if (!string.IsNullOrEmpty(directoryName))
                        {
                            Directory.CreateDirectory(directoryName);
                        }

                        var fileName = Path.GetFileName(fullZipToPath);
                        if (fileName.Length == 0)
                        {
                            zipEntry = zipInputStream.GetNextEntry();
                            continue;
                        }

                        var buffer = new byte[4096]; // 4K is optimum
                        using (var streamWriter = File.Create(fullZipToPath))
                        {
                            StreamUtils.Copy(zipInputStream, streamWriter, buffer);
                        }

                        zipEntry = zipInputStream.GetNextEntry();
                    }
                }
            }

            Logger.LogInformation($"'{moduleName}' has been successfully downloaded to '{outputFolder}'");
        }
    }
}
