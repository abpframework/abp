using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Microsoft.AspNetCore.RequestLocalization;

public class AbpRequestCultureResolver : ITransientDependency
{
    protected IAbpRequestLocalizationOptionsProvider OptionsProvider { get; }

    public AbpRequestCultureResolver(IAbpRequestLocalizationOptionsProvider optionsProvider)
    {
        OptionsProvider = optionsProvider;
    }

    public virtual async Task<CultureInfo?> ResolveAsync(string? name, bool? uiCulture = null)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return null;
        }

        var options = await OptionsProvider.GetLocalizationOptionsAsync();
        if (uiCulture != true)
        {
            var culture = options.SupportedCultures?.FirstOrDefault(c => string.Equals(c.Name, name, StringComparison.OrdinalIgnoreCase));
            if (culture != null)
            {
                return culture;
            }

            if (string.Equals(options.DefaultRequestCulture.Culture.Name, name, StringComparison.OrdinalIgnoreCase))
            {
                return options.DefaultRequestCulture.Culture;
            }
        }

        if (uiCulture != false)
        {
            return options.SupportedUICultures?.FirstOrDefault(c => string.Equals(c.Name, name, StringComparison.OrdinalIgnoreCase))
                ?? (string.Equals(options.DefaultRequestCulture.UICulture.Name, name, StringComparison.OrdinalIgnoreCase)
                    ? options.DefaultRequestCulture.UICulture
                    : null);
        }

        return null;
    }
}
