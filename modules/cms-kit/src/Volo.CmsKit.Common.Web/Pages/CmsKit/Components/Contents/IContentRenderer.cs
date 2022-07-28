using System.Threading.Tasks;

namespace Volo.CmsKit.Web.Pages.CmsKit.Components.Contents;

public interface IContentRenderer
{
    Task<string> RenderAsync(string value);
}
