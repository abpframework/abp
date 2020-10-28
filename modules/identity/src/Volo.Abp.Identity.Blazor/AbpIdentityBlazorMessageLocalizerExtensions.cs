using System.Collections.Generic;
using System.Linq;
using Blazorise;
using Microsoft.Extensions.Localization;

namespace Volo.Abp.Identity.Blazor
{
    public static class AbpIdentityBlazorMessageLocalizerExtensions
    {
        public static string Localize<T>(this IStringLocalizer<T> stringLocalizer, string format, IEnumerable<string> arguments)
        {
            try
            {
                return arguments?.Count() > 0
                    ? stringLocalizer[format, LocalizeMessageArguments(stringLocalizer, arguments)?.ToArray()]
                    : stringLocalizer[format];
            }
            catch
            {
                return stringLocalizer[format];
            }
        }

        private static IEnumerable<string> LocalizeMessageArguments<T>(IStringLocalizer<T> stringLocalizer, IEnumerable<string> arguments)
        {
            foreach (var argument in arguments)
            {
                yield return stringLocalizer[$"DisplayName:{argument}"]
                    ?? stringLocalizer[argument]
                    ?? argument;
            }
        }
    }
}
