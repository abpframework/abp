using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Localization;

namespace Volo.Abp.AspNetCore.Components.Web;

public class AbpBlazorMessageLocalizerHelper<T>
{
    private readonly IStringLocalizer<T> stringLocalizer;

    public AbpBlazorMessageLocalizerHelper(IStringLocalizer<T> stringLocalizer)
    {
        this.stringLocalizer = stringLocalizer;
    }

    public string Localize(string message, IEnumerable<string>? arguments = null)
    {
        try
        {
            var argumentsList = arguments?.ToList();
            return argumentsList?.Count > 0
                ? stringLocalizer[message, LocalizeMessageArguments(argumentsList).ToArray()]
                : stringLocalizer[message];
        }
        catch
        {
            return stringLocalizer[message];
        }
    }

    private IEnumerable<object> LocalizeMessageArguments(List<string> arguments)
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
