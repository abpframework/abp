using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Volo.Abp.Json.Newtonsoft;

namespace Volo.Abp.AspNetCore.Mvc.NewtonsoftJson;

public class AbpMvcNewtonsoftJsonOptionsSetup : IConfigureOptions<MvcNewtonsoftJsonOptions>
{
    private readonly IServiceProvider _serviceProvider;

    public AbpMvcNewtonsoftJsonOptionsSetup(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public void Configure(MvcNewtonsoftJsonOptions options)
    {
        options.SerializerSettings.ContractResolver = _serviceProvider.GetRequiredService<AbpCamelCasePropertyNamesContractResolver>();

        var converters = _serviceProvider.GetRequiredService<IOptions<AbpNewtonsoftJsonSerializerOptions>>().Value
            .Converters.Select(converterType => _serviceProvider.GetRequiredService(converterType).As<JsonConverter>())
            .ToList();

        options.SerializerSettings.Converters.InsertRange(0, converters);
    }
}
