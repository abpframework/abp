using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.Extensions.Localization;

namespace Volo.Abp.Identity.Blazor
{
    public class AbpIdentityBlazorMessageLocalizerHelper<T>
    {
        private readonly IStringLocalizer<T> stringLocalizer;

        public AbpIdentityBlazorMessageLocalizerHelper(IStringLocalizer<T> stringLocalizer)
        {
            this.stringLocalizer = stringLocalizer;
        }

        public string Localize(string message, [CanBeNull] IEnumerable<string> arguments)
        {
            try
            {
                return arguments?.Count() > 0
                    ? stringLocalizer[message, LocalizeMessageArguments(arguments)?.ToArray()]
                    : stringLocalizer[message];
            }
            catch
            {
                return stringLocalizer[message];
            }
        }

        private IEnumerable<string> LocalizeMessageArguments(IEnumerable<string> arguments)
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
