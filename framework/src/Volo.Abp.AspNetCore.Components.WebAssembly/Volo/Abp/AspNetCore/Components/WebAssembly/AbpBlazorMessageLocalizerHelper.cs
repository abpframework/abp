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
                string localization = stringLocalizer[$"DisplayName:{argument}"];

                if (!string.IsNullOrEmpty(localization) && !localization.StartsWith("DisplayName:"))
                    yield return localization;

                // then try to localize with just "{Name}"
                localization = stringLocalizer[argument];

                if (!string.IsNullOrEmpty(localization) && localization != argument)
                    yield return localization;

                // no localization found so just return what we got
                yield return argument;
            }
        }
    }
}
