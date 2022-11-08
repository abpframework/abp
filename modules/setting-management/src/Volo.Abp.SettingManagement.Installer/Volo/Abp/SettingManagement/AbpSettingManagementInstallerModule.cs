﻿using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.SettingManagement;

[DependsOn(
    typeof(AbpVirtualFileSystemModule)
    )]
public class AbpSettingManagementInstallerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpSettingManagementInstallerModule>();
        });
    }
}
