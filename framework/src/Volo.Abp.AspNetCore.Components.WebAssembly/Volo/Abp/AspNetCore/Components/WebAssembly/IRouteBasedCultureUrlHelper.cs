using System.Threading.Tasks;

namespace Volo.Abp.AspNetCore.Components.WebAssembly;

public interface IRouteBasedCultureUrlHelper
{
    /// <summary>
    /// Prepends the current culture to <paramref name="url"/> when route-based culture is enabled
    /// and the current culture is a known application language.
    /// Returns the original <paramref name="url"/> unchanged when the feature is disabled or the
    /// culture is not recognised.
    /// </summary>
    Task<string> PrependCulturePrefixAsync(string url);
}
