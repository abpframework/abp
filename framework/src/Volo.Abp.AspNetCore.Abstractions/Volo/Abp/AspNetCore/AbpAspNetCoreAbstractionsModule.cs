using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.VirtualFileSystem;
using Volo.Abp.AspNetCore.WebClientInfo;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore;

public class AbpAspNetCoreAbstractionsModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddSingleton<IWebContentFileProvider, NullWebContentFileProvider>();
        context.Services.AddSingleton<IWebClientInfoProvider, NullWebClientInfoProvider>();;
    }
}
