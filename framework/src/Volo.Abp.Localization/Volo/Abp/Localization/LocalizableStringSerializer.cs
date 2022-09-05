using System;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Localization;

public class LocalizableStringSerializer : ILocalizableStringSerializer, ITransientDependency
{
    protected AbpLocalizationOptions LocalizationOptions { get; }

    public LocalizableStringSerializer(IOptions<AbpLocalizationOptions> localizationOptions)
    {
        LocalizationOptions = localizationOptions.Value;
    }

    public virtual string Serialize(ILocalizableString localizableString)
    {
        if (localizableString is LocalizableString realLocalizableString)
        {
            return $"L:{LocalizationResourceNameAttribute.GetName(realLocalizableString.ResourceType)},{realLocalizableString.Name}";
        }

        if (localizableString is FixedLocalizableString fixedLocalizableString)
        {
            return $"F:{fixedLocalizableString.Value}";
        }

        throw new AbpException($"Unknown {nameof(ILocalizableString)} type: {localizableString.GetType().FullName}");
    }

    public virtual ILocalizableString Deserialize(string value)
    {
        if (value.IsNullOrEmpty() ||
            value.Length < 3 ||
            value[1] != ':')
        {
            return new FixedLocalizableString(value);
        }
        
        var type = value[0];
        switch (type)
        {
            case 'F':
                return new FixedLocalizableString(value.Substring(2));
            case 'L':
                var commaPosition = value.IndexOf(',', 2);
                if (commaPosition == -1)
                {
                    throw new AbpException("Invalid LocalizableString value: " + value);
                }
                
                var resourceName = value.Substring(2, commaPosition - 2);
                var name = value.Substring(commaPosition + 1);
                if (name.IsNullOrWhiteSpace())
                {
                    throw new AbpException("Invalid LocalizableString value: " + value);
                }
                
                var resourceType = LocalizationOptions.Resources.GetOrNull(resourceName)?.ResourceType;
                    
                return new LocalizableString(
                    resourceType,
                    name
                );
            default:
                return new FixedLocalizableString(value);
        }
    }
}