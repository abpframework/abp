using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Volo.Abp.AspNetCore.Mvc.Json;

public static class MvcCoreBuilderExtensions
{
    public static IMvcCoreBuilder AddAbpHybridJson(this IMvcCoreBuilder builder)
    {
        builder.Services.TryAddEnumerable(ServiceDescriptor.Transient<IConfigureOptions<JsonOptions>, AbpJsonOptionsSetup>());

        builder.Services.Configure<MvcOptions>(options =>
        {
            options.InputFormatters.RemoveType<SystemTextJsonInputFormatter>();
            options.InputFormatters.Add(new AbpHybridJsonInputFormatter());

            options.OutputFormatters.RemoveType<SystemTextJsonOutputFormatter>();
            options.OutputFormatters.Add(new AbpHybridJsonOutputFormatter());
        });

        builder.Services.Configure<AbpHybridJsonFormatterOptions>(options =>
        {
            options.TextInputFormatters.Add<AbpSystemTextJsonFormatter>();
            options.TextOutputFormatters.Add<AbpSystemTextJsonFormatter>();
        });

        return builder;
    }
}
