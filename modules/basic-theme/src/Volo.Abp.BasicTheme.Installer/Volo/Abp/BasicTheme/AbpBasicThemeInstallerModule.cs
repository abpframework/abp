﻿using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.BasicTheme;

[DependsOn(
    typeof(AbpVirtualFileSystemModule)
    )]
public class AbpBasicThemeInstallerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpBasicThemeInstallerModule>();
        });
    }
}
