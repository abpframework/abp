using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Json;
using Volo.Abp.Json.SystemTextJson;

namespace Volo.Abp.FeatureManagement;

public class NewtonsoftStringValueJsonConverter_Tests : StringValueJsonConverter_Tests
{
    protected override void AfterAddApplication(IServiceCollection services)
    {
        services.PreConfigure<AbpJsonOptions>(options =>
        {
            options.Providers.Remove<AbpSystemTextJsonSerializerProvider>();
        });
    }
}
