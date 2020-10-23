using System.Collections.Generic;
using System.Linq;
using Blazorise;
using Microsoft.Extensions.Localization;

namespace Volo.Abp.Identity.Blazor
{
    public static class AbpIdentityBlazorMessageLocalizerExtensions
    {
        public static IEnumerable<string> Localize<T>(this IStringLocalizer<T> stringLocalizer, ValidationMessageLocalizerEventArgs eventArgs)
        {
            return LocalizeMessages(stringLocalizer, eventArgs.Messages?.Select(m => (m.Message, m.MessageArguments)))?.ToArray();
        }

        private static IEnumerable<string> LocalizeMessages<T>(IStringLocalizer<T> stringLocalizer, IEnumerable<(string format, string[] arguments)> messages)
        {
            return messages?.Select(m => LocalizeMessage(stringLocalizer, m.format, m.arguments));
        }

        private static string LocalizeMessage<T>(IStringLocalizer<T> stringLocalizer, string format, params string[] arguments)
        {
            try
            {
                return arguments?.Length > 0
                    ? string.Format(stringLocalizer[format], LocalizeMessageArguments(stringLocalizer, arguments)?.ToArray())
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
