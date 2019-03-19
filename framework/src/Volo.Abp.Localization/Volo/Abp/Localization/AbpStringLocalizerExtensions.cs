using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.Localization;
using Volo.Abp.DynamicProxy;

namespace Volo.Abp.Localization
{
    public static class AbpStringLocalizerExtensions
    {
        public static IStringLocalizer GetInternalLocalizer(this IStringLocalizer stringLocalizer)
        {
            var localizerField = stringLocalizer.GetType()
                .GetField(
                    "_localizer",
                    BindingFlags.Instance | 
                    BindingFlags.NonPublic
                );

            if (localizerField == null)
            {
                return stringLocalizer;
            }

            return localizerField.GetValue(stringLocalizer) as IStringLocalizer;
        }

        public static IEnumerable<LocalizedString> GetAllStrings(
            this IStringLocalizer stringLocalizer,
            bool includeParentCultures,
            bool includeBaseLocalizers)
        {
            var internalLocalizer = (ProxyHelper.UnProxy(stringLocalizer) as IStringLocalizer).GetInternalLocalizer();
            if (internalLocalizer is IStringLocalizerSupportsInheritance stringLocalizerSupportsInheritance)
            {
                return stringLocalizerSupportsInheritance.GetAllStrings(
                    includeParentCultures,
                    includeBaseLocalizers
                );
            }

            return stringLocalizer.GetAllStrings(
                includeParentCultures
            );
        }
    }
}