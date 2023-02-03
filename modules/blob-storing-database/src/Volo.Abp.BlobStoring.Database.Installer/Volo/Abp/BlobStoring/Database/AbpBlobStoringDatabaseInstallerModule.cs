﻿using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.BlobStoring.Database;

[DependsOn(
    typeof(AbpVirtualFileSystemModule)
    )]
public class AbpBlobStoringDatabaseInstallerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpBlobStoringDatabaseInstallerModule>();
        });
    }
}
