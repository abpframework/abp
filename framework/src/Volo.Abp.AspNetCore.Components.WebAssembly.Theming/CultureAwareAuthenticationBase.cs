using Microsoft.AspNetCore.Components;
using Volo.Abp.AspNetCore.Components;

namespace Volo.Abp.AspNetCore.Components.WebAssembly.Theming;

/// <summary>
/// Shared base for WASM theme <c>Authentication</c> pages.
/// Provides a <see cref="GetCultureAwareHomeUrl"/> helper so the culture-aware
/// home URL construction is not duplicated across theme packages.
/// </summary>
public abstract class CultureAwareAuthenticationBase : AbpComponentBase
{
    [Inject]
    protected NavigationManager Navigation { get; set; } = default!;

    [Parameter]
    public string? Culture { get; set; }

    protected virtual string GetCultureAwareHomeUrl()
    {
        return string.IsNullOrEmpty(Culture)
            ? Navigation.BaseUri
            : Navigation.BaseUri + Culture + "/";
    }
}
