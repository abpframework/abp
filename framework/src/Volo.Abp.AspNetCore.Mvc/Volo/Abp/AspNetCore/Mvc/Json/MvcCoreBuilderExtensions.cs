using System.Buffers;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using Volo.Abp.Json;

namespace Volo.Abp.AspNetCore.Mvc.Json
{
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

            //SystemTextJsonInputFormatter
            builder.Services.AddTransient(provider =>
            {
                var jsonOptions = provider.GetRequiredService<IOptions<JsonOptions>>();
                var logger = provider.GetRequiredService<ILoggerFactory>().CreateLogger<SystemTextJsonInputFormatter>();
                return new SystemTextJsonInputFormatter(jsonOptions.Value, logger);
            });

            builder.Services.TryAddTransient<DefaultObjectPoolProvider>();
            //NewtonsoftJsonInputFormatter
            builder.Services.AddTransient(provider =>
            {
                var jsonOptions = provider.GetRequiredService<IOptions<MvcNewtonsoftJsonOptions>>().Value;

                return new NewtonsoftJsonInputFormatter(
                    provider.GetRequiredService<ILoggerFactory>().CreateLogger<NewtonsoftJsonInputFormatter>(),
                    jsonOptions.SerializerSettings,
                    provider.GetRequiredService<ArrayPool<char>>(),
                    provider.GetRequiredService<DefaultObjectPoolProvider>(),
                    provider.GetRequiredService<IOptions<MvcOptions>>().Value,
                    jsonOptions);
            });

            //SystemTextJsonOutputFormatter
            builder.Services.AddTransient(provider =>
            {
                var jsonSerializerOptions = provider.GetRequiredService<IOptions<JsonOptions>>().Value.JsonSerializerOptions;
                if (jsonSerializerOptions.Encoder is null)
                {
                    // If the user hasn't explicitly configured the encoder, use the less strict encoder that does not encode all non-ASCII characters.
                    jsonSerializerOptions = new JsonSerializerOptions(jsonSerializerOptions)
                    {
                        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                    };
                }
                return new SystemTextJsonOutputFormatter(jsonSerializerOptions);
            });

            //NewtonsoftJsonOutputFormatter
            builder.Services.AddTransient(provider =>
            {
                var jsonOptions = provider.GetRequiredService<IOptions<MvcNewtonsoftJsonOptions>>().Value;
                return new NewtonsoftJsonOutputFormatter(
                    jsonOptions.SerializerSettings,
                    provider.GetRequiredService<ArrayPool<char>>(),
                    provider.GetRequiredService<IOptions<MvcOptions>>().Value);
            });

            builder.Services.TryAddEnumerable(ServiceDescriptor.Transient<IConfigureOptions<JsonOptions>, AbpJsonOptionsSetup>());
            builder.Services.TryAddEnumerable(ServiceDescriptor.Transient<IConfigureOptions<MvcNewtonsoftJsonOptions>, AbpMvcNewtonsoftJsonOptionsSetup>());

            builder.Services.Configure<MvcOptions>(options =>
            {
                options.InputFormatters.RemoveType<SystemTextJsonInputFormatter>();
                options.InputFormatters.RemoveType<NewtonsoftJsonInputFormatter>();
                options.InputFormatters.Add(new AbpHybridJsonInputFormatter());

                options.OutputFormatters.RemoveType<SystemTextJsonOutputFormatter>();
                options.OutputFormatters.RemoveType<NewtonsoftJsonOutputFormatter>();
                options.OutputFormatters.Add(new AbpHybridJsonOutputFormatter());
            });

            return builder;
        }
    }
}
