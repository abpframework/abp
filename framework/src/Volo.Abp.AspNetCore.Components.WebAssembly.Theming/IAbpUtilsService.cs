using System.Threading.Tasks;

namespace Volo.Abp.AspNetCore.Components.WebAssembly.Theming
{
    public interface IAbpUtilsService
    {
        ValueTask AddClassToTagAsync(string tagName, string className);

        ValueTask RemoveClassFromTagAsync(string tagName, string className);

        ValueTask<bool> HasClassOnTagAsync(string tagName, string className);

        ValueTask ReplaceLinkHrefByIdAsync(string linkId, string hrefValue);
    }
}
