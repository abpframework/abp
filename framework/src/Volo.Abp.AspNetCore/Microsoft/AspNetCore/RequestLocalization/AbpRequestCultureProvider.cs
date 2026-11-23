using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Primitives;

namespace Microsoft.AspNetCore.RequestLocalization;

public class AbpRequestCultureProvider : IRequestCultureProvider
{
    // Resolve parent cultures from a finite set, before ASP.NET Core can cache request-supplied names.
    private static readonly Lazy<Dictionary<string, CultureInfo>> KnownCultures = new(CreateKnownCultures);

    private static Dictionary<string, CultureInfo> CreateKnownCultures()
    {
        var cultures = CultureInfo.GetCultures(CultureTypes.AllCultures)
            .GroupBy(culture => culture.Name, StringComparer.OrdinalIgnoreCase)
            .ToDictionary(group => group.Key, group => group.First(), StringComparer.OrdinalIgnoreCase);

        // Some platforms enumerate zh-Hant-TW but not its standard zh-TW alias.
        foreach (var culture in cultures.Values.ToArray())
        {
            var parts = culture.Name.Split('-');
            if (parts.Length != 3 || parts[1].Length != 4 || parts[2].Length is not (2 or 3))
            {
                continue;
            }

            var alias = parts[0] + "-" + parts[2];
            if (!cultures.ContainsKey(alias))
            {
                try
                {
                    cultures.Add(alias, new CultureInfo(alias));
                }
                catch (CultureNotFoundException)
                {
                    // The alias is not available on this platform.
                }
            }
        }

        return cultures;
    }

    public IRequestCultureProvider InnerProvider { get; }
    private readonly RequestLocalizationOptions _options;

    public AbpRequestCultureProvider(IRequestCultureProvider innerProvider, RequestLocalizationOptions options)
    {
        InnerProvider = innerProvider;
        _options = options;
    }

    public virtual async Task<ProviderCultureResult?> DetermineProviderCultureResult(HttpContext httpContext)
    {
        var result = await InnerProvider.DetermineProviderCultureResult(httpContext);
        if (result == null)
        {
            return null;
        }

        var cultures = Resolve(result.Cultures, _options.SupportedCultures, _options.FallBackToParentCultures);
        var uiCultures = Resolve(result.UICultures, _options.SupportedUICultures, _options.FallBackToParentUICultures);
        return cultures.Count == 0 && uiCultures.Count == 0
            ? null
            : new ProviderCultureResult(cultures, uiCultures);
    }

    private static List<StringSegment> Resolve(IList<StringSegment> names, IList<CultureInfo>? supportedCultures, bool fallback)
    {
        var result = new List<StringSegment>();
        if (supportedCultures == null)
        {
            return result;
        }

        foreach (var name in names)
        {
            var candidate = name.Value;
            for (var depth = 0; !string.IsNullOrEmpty(candidate) && depth <= 5; depth++)
            {
                var supported = supportedCultures.FirstOrDefault(culture =>
                    string.Equals(culture.Name, candidate, StringComparison.OrdinalIgnoreCase));
                if (supported != null)
                {
                    result.Add(supported.Name);
                    break;
                }

                if (!fallback || !KnownCultures.Value.TryGetValue(candidate, out var knownCulture))
                {
                    break;
                }

                candidate = knownCulture.Parent.Name;
            }
        }

        return result;
    }
}
