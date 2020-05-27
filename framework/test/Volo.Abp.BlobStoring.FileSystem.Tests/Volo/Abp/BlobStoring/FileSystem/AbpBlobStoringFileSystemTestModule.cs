using System;
using System.IO;
using Volo.Abp.Modularity;

namespace Volo.Abp.BlobStoring.FileSystem
{
    [DependsOn(
        typeof(AbpBlobStoringFileSystemModule)
        )]
    public class AbpBlobStoringFileSystemTestModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpBlobStoringOptions>(options =>
            {
                options.Containers.ConfigureDefault(container =>
                {
                    container.UseFileSystem(fileSystem =>
                    {
                        fileSystem.BasePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
                    });
                });
            });
        }
    }
}