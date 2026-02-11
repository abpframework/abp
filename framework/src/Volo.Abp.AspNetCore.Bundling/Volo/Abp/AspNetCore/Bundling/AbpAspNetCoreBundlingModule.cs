using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.Minify;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.AspNetCore.Bundling;

[DependsOn(
    typeof(AbpAspNetCoreMvcUiBundlingAbstractionsModule),
    typeof(AbpMinifyModule),
    typeof(AbpVirtualFileSystemModule)
)]
public class AbpAspNetCoreBundlingModule : AbpModule
{
}