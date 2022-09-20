using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.Localization;

namespace Volo.Abp.AspNetCore.Mvc.Client;

public class RemoteLocalizationContributor : ILocalizationResourceContributor
{
    private LocalizationResource _resource;
    private ICachedApplicationConfigurationClient _applicationConfigurationClient;
    private ILogger<RemoteLocalizationContributor> _logger;

    public void Initialize(LocalizationResourceInitializationContext context)
    {
        _resource = context.Resource;
        _applicationConfigurationClient = context.ServiceProvider.GetRequiredService<ICachedApplicationConfigurationClient>();
        _logger = context.ServiceProvider.GetService<ILogger<RemoteLocalizationContributor>>()
                  ?? NullLogger<RemoteLocalizationContributor>.Instance;
    }

    public LocalizedString GetOrNull(string cultureName, string name)
    {
        var resource = GetResourceOrNull();
        if (resource == null)
        {
            return null;
        }

        var value = resource.GetOrDefault(name);
        if (value == null)
        {
            return null;
        }

        return new LocalizedString(name, value);
    }

    public void Fill(string cultureName, Dictionary<string, LocalizedString> dictionary)
    {
        var resource = GetResourceOrNull();
        if (resource == null)
        {
            return;
        }

        foreach (var keyValue in resource)
        {
            dictionary[keyValue.Key] = new LocalizedString(keyValue.Key, keyValue.Value);
        }
    }

    private Dictionary<string, string> GetResourceOrNull()
    {
        var applicationConfigurationDto = _applicationConfigurationClient.Get();

        var resource = applicationConfigurationDto
            .Localization.Values
            .GetOrDefault(_resource.ResourceName);

        if (resource == null)
        {
            _logger.LogWarning($"Could not find the localization resource {_resource.ResourceName} on the remote server!");
        }

        return resource;
    }
}
