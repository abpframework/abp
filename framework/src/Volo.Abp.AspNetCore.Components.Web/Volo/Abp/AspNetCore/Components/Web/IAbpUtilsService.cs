using System.Threading.Tasks;

namespace Volo.Abp.AspNetCore.Components.Web;

public interface IAbpUtilsService
{
    ValueTask AddClassToTagAsync(string tagName, string className);

    ValueTask RemoveClassFromTagAsync(string tagName, string className);

    ValueTask<bool> HasClassOnTagAsync(string tagName, string className);

    ValueTask ReplaceLinkHrefByIdAsync(string linkId, string hrefValue);

    ValueTask ToggleFullscreenAsync();

    ValueTask RequestFullscreenAsync();

    ValueTask ExitFullscreenAsync();
}
