﻿using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.CmsKit;

[DependsOn(
    typeof(AbpVirtualFileSystemModule)
    )]
public class VoloCmsKitInstallerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<VoloCmsKitInstallerModule>();
        });
    }
}
