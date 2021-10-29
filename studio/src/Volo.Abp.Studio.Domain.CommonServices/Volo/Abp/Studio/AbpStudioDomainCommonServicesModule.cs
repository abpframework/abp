using System.IO.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Volo.Abp.Studio
{
    [DependsOn(
        typeof(AbpStudioDomainSharedModule)
        )]
    public class AbpStudioDomainCommonServicesModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddSingleton<IFileSystem>(new FileSystem());
        }
    }
}
