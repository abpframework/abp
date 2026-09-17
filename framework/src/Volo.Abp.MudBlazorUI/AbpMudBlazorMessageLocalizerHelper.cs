using System;
using Microsoft.Extensions.Localization;

namespace Volo.Abp.MudBlazorUI;

public class AbpMudBlazorMessageLocalizerHelper<T>
{
    private readonly IStringLocalizer<T> _localizer;

    public AbpMudBlazorMessageLocalizerHelper(IStringLocalizer<T> localizer)
    {
        _localizer = localizer;
    }

    public string Localize(string message, params object[] arguments)
    {
        return _localizer[message, arguments];
    }

    public string? LocalizeOrNull(string? message, params object[] arguments)
    {
        if (message.IsNullOrEmpty())
        {
            return null;
        }

        return _localizer[message!, arguments];
    }
}
