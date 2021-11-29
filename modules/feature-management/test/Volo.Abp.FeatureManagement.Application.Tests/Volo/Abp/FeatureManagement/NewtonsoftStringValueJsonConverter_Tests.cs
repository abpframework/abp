using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Json;

namespace Volo.Abp.FeatureManagement
{
    public class NewtonsoftStringValueJsonConverter_Tests : StringValueJsonConverter_Tests
    {
        protected override void AfterAddApplication(IServiceCollection services)
        {
            services.PreConfigure<AbpJsonOptions>(options =>
            {
                options.UseHybridSerializer = true;
            });
        }
    }
}
