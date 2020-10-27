using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Volo.Abp.Json.Microsoft;
using Volo.Abp.Json.Newtonsoft;
using Volo.Abp.Modularity;

namespace Volo.Abp.Json
{
    [DependsOn(typeof(AbpJsonModule))]
    public class AbpJsonNewtonsoftModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpNewtonsoftJsonSerializerOptions>(options =>
            {
                options.Converters.Add<AbpJsonIsoDateTimeConverter>();
            });
        }
    }
}
