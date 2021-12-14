using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.ObjectPool;
using Volo.Abp.Json;

namespace Volo.Abp.AspNetCore.Mvc.Json;

public static class MvcCoreBuilderExtensions
{
    public static IMvcCoreBuilder AddAbpHybridJson(this IMvcCoreBuilder builder)
    {
        var abpJsonOptions = builder.Services.ExecutePreConfiguredActions<AbpJsonOptions>();
        if (!abpJsonOptions.UseHybridSerializer)
        {
            builder.Services.TryAddEnumerable(ServiceDescriptor.Transient<IConfigureOptions<MvcNewtonsoftJsonOptions>, AbpMvcNewtonsoftJsonOptionsSetup>());
            builder.AddNewtonsoftJson();
            return builder;
        }

        builder.Services.TryAddTransient<DefaultObjectPoolProvider>();
        builder.Services.TryAddEnumerable(ServiceDescriptor.Transient<IConfigureOptions<JsonOptions>, AbpJsonOptionsSetup>());
        builder.Services.TryAddEnumerable(ServiceDescriptor.Transient<IConfigureOptions<MvcNewtonsoftJsonOptions>, AbpMvcNewtonsoftJsonOptionsSetup>());
        builder.Services.TryAddEnumerable(ServiceDescriptor.Transient<IConfigureOptions<MvcOptions>, AbpHybridJsonOptionsSetup>());
        return builder;
    }
}
