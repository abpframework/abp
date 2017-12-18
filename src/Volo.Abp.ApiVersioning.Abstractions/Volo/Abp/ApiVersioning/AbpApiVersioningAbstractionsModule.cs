using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Volo.Abp.ApiVersioning
{
    public class AbpApiVersioningAbstractionsModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IRequestedApiVersion>(NullRequestedApiVersion.Instance);
        }
    }
}
