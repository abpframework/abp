using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;
using Volo.Abp.Localization;

namespace Volo.Abp.AspNetCore.Mvc.Client;

public class RemoteLocalizationContributor : ILocalizationResourceContributor
{
    public bool IsDynamic => true;

    private LocalizationResourceBase _resource = default!;
    private ICachedApplicationConfigurationClient _applicationConfigurationClient = default!;
    private ILogger<RemoteLocalizationContributor> _logger = default!;

    public void Initialize(LocalizationResourceInitializationContext context)
    {
        _resource = context.Resource;
        _applicationConfigurationClient = context.ServiceProvider.GetRequiredService<ICachedApplicationConfigurationClient>();
        _logger = context.ServiceProvider.GetService<ILogger<RemoteLocalizationContributor>>()
                  ?? NullLogger<RemoteLocalizationContributor>.Instance;
    }

    public virtual LocalizedString? GetOrNull(string cultureName, string name)
    {
        /* cultureName is not used because remote localization can only
         * be done in the current culture. */
        
        return GetOrNullInternal(_resource.ResourceName, name);
    }
    
    protected virtual LocalizedString? GetOrNullInternal(string resourceName, string name)
    {
        var resource = GetResourceOrNull(resourceName);
        if (resource == null)
        {
            return null;
        }

        var value = resource.Texts.GetOrDefault(name);
        if (value != null)
        {
            return new LocalizedString(name, value);
        }

        foreach (var baseResource in resource.BaseResources)
        {
            value = GetOrNullInternal(baseResource, name)?.ToString();
            if (value != null)
            {
                return new LocalizedString(name, value);
            }
        }
            
        return null;
    }

    public virtual void Fill(string cultureName, Dictionary<string, LocalizedString> dictionary)
    {
        /* cultureName is not used because remote localization can only
         * be done in the current culture. */
        
        FillInternal(_resource.ResourceName, dictionary);
    }
    
    protected virtual void FillInternal(string resourceName, Dictionary<string, LocalizedString> dictionary)
    {
        var resource = GetResourceOrNull(resourceName);
        if (resource == null)
        {
            return;
        }

        foreach (var baseResource in resource.BaseResources)
        {
            FillInternal(baseResource, dictionary);
        }

        foreach (var keyValue in resource.Texts)
        {
            dictionary[keyValue.Key] = new LocalizedString(keyValue.Key, keyValue.Value);
        }
    }

    public virtual async Task FillAsync(string cultureName, Dictionary<string, LocalizedString> dictionary)
    {
        /* cultureName is not used because remote localization can only
         * be done in the current culture. */

        await FillInternalAsync(_resource.ResourceName, dictionary);
    }
    
    protected virtual async Task FillInternalAsync(string resourceName, Dictionary<string, LocalizedString> dictionary)
    {
        var resource = await GetResourceOrNullAsync(resourceName);
        if (resource == null)
        {
            return;
        }
        
        foreach (var baseResource in resource.BaseResources)
        {
            await FillInternalAsync(baseResource, dictionary);
        }

        foreach (var keyValue in resource.Texts)
        {
            dictionary[keyValue.Key] = new LocalizedString(keyValue.Key, keyValue.Value);
        }
    }

    public virtual Task<IEnumerable<string>> GetSupportedCulturesAsync()
    {
        /* This contributor does not know all the supported cultures by the
         remote localization resource, and it is not needed on the client side */
        return Task.FromResult((IEnumerable<string>)Array.Empty<string>());
    }

    protected virtual ApplicationLocalizationResourceDto? GetResourceOrNull(string resourceName)
    {
        var applicationConfigurationDto = _applicationConfigurationClient.Get();
        return GetResourceOrNull(applicationConfigurationDto, resourceName);
    }
    
    protected virtual async Task<ApplicationLocalizationResourceDto?> GetResourceOrNullAsync(string resourceName)
    {
        var applicationConfigurationDto = await _applicationConfigurationClient.GetAsync();
        return GetResourceOrNull(applicationConfigurationDto, resourceName);
    }

    protected virtual ApplicationLocalizationResourceDto? GetResourceOrNull(
        ApplicationConfigurationDto applicationConfigurationDto,
        string resourceName)
    {
        var resource = applicationConfigurationDto.Localization.Resources.GetOrDefault(resourceName);
        if (resource != null)
        {
            return resource;
        }

        var legacyResource = applicationConfigurationDto
            .Localization.Values
            .GetOrDefault(resourceName);

        if (legacyResource != null)
        {
            return new ApplicationLocalizationResourceDto
            {
                Texts = legacyResource,
                BaseResources = Array.Empty<string>()
            };
        }

        _logger.LogWarning($"Could not find the localization resource {resourceName} on the remote server!");
        return null;
    }
}
