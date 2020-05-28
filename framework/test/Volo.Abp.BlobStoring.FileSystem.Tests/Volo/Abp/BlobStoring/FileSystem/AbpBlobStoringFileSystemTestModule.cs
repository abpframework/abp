using System;
using System.IO;
using Volo.Abp.IO;
using Volo.Abp.Modularity;

namespace Volo.Abp.BlobStoring.FileSystem
{
    [DependsOn(
        typeof(AbpBlobStoringFileSystemModule),
        typeof(AbpBlobStoringTestModule)
        )]
    public class AbpBlobStoringFileSystemTestModule : AbpModule
    {
        private readonly string _testDirectoryPath;

        public AbpBlobStoringFileSystemTestModule()
        {
            _testDirectoryPath = Path.Combine(
                Path.GetTempPath(),
                Guid.NewGuid().ToString("N")
            );
        }
        
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpBlobStoringOptions>(options =>
            {
                options.Containers.ConfigureAll((containerName, containerConfiguration) =>
                {
                    containerConfiguration.UseFileSystem(fileSystem =>
                    {
                        fileSystem.BasePath = _testDirectoryPath;
                    });
                });
            });
        }

        public override void OnApplicationShutdown(ApplicationShutdownContext context)
        {
            DirectoryHelper.DeleteIfExists(_testDirectoryPath, true);
        }
    }
}