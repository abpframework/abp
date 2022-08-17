using System;
using Microsoft.Extensions.Options;
using Volo.Abp.Json.SystemTextJson.Modifiers;

namespace Volo.Abp.Json.SystemTextJson;

public class AbpSystemTextJsonSerializerModifiersOptionsSetup : IConfigureOptions<AbpSystemTextJsonSerializerModifiersOptions>
{
    protected IServiceProvider ServiceProvider { get; }

    public AbpSystemTextJsonSerializerModifiersOptionsSetup(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
    }

    public void Configure(AbpSystemTextJsonSerializerModifiersOptions options)
    {
        options.Modifiers.Add(new AbpDateTimeConverterModifier().CreateModifyAction(ServiceProvider));
    }
}
