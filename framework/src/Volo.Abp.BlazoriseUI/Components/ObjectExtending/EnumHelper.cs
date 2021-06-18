using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Localization;

namespace Volo.Abp.BlazoriseUI.Components.ObjectExtending
{
    public static class EnumHelper
    {
        public static string GetLocalizedMemberName(Type enumType, object value, IStringLocalizerFactory stringLocalizerFactory)
        {
            var memberName = enumType.GetEnumName(value);
            var localizedMemberName = AbpInternalLocalizationHelper.LocalizeWithFallback(
                new[]
                {
                        stringLocalizerFactory.CreateDefaultOrNull()
                },
                new[]
                {
                        $"Enum:{enumType.Name}.{memberName}",
                        $"{enumType.Name}.{memberName}",
                        memberName
                },
                memberName
            );

            return localizedMemberName;
        }
    }
}
