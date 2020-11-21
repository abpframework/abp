using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.Extensions.Localization;

namespace Volo.Abp.AspNetCore.Components.WebAssembly
{
    public class AbpBlazorMessageLocalizerHelper<T>
    {
        private readonly IStringLocalizer<T> stringLocalizer;

        public AbpBlazorMessageLocalizerHelper(IStringLocalizer<T> stringLocalizer)
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
                // first try to localize with "DisplayName:{Name}"
                var localization = stringLocalizer[$"DisplayName:{argument}"];

                if (localization.ResourceNotFound)
                {
                    // then try to localize with just "{Name}"
                    localization = stringLocalizer[argument];
                }

                yield return localization;
            }
        }
    }
}
