using System;
using JetBrains.Annotations;
using Volo.Abp.Localization;

namespace Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations.ObjectExtending
{
    [Serializable]
    public class LocalizedDisplayNameDto
    {
        public string Name { get; set; }

        public string Resource { get; set; }

        [CanBeNull]
        public static LocalizedDisplayNameDto CreateOrNull(ILocalizableString localizableString)
        {
            if (localizableString is LocalizableString localizableStringInstance)
            {
                return new LocalizedDisplayNameDto
                {
                    Name = localizableStringInstance.Name,
                    Resource = LocalizationResourceNameAttribute.GetName(localizableStringInstance.ResourceType)
                };
            }

            if (localizableString is FixedLocalizableString fixedLocalizableString)
            {
                return new LocalizedDisplayNameDto
                {
                    Name = fixedLocalizableString.Value,
                };
            }

            return null;
        }
    }
}