using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Volo.Abp.Json.Newtonsoft;
using Volo.Abp.Json.SystemTextJson;
using Volo.Abp.Modularity;
using Volo.Abp.Timing;

namespace Volo.Abp.Json
{
    [DependsOn(typeof(AbpTimingModule))]
    public class AbpJsonModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.TryAddEnumerable(ServiceDescriptor
                .Transient<IConfigureOptions<AbpSystemTextJsonSerializerOptions>, AbpSystemTextJsonSerializerOptionsSetup>());

            Configure<AbpJsonOptions>(options =>
            {
                options.Providers.Add<AbpNewtonsoftJsonSerializerProvider>();

                var abpJsonOptions = context.Services.ExecutePreConfiguredActions<AbpJsonOptions>();
                if (abpJsonOptions.UseHybridSerializer)
                {
                    options.Providers.Add<AbpSystemTextJsonSerializerProvider>();
                }
            });

            Configure<AbpNewtonsoftJsonSerializerOptions>(options =>
            {
                options.Converters.Add<AbpJsonIsoDateTimeConverter>();
            });
        }
    }
}
