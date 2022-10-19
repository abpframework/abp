using System.Threading.Tasks;

namespace Volo.CmsKit.Public.Web.Renderers;

public interface IMarkdownToHtmlRenderer
{
    Task<string> RenderAsync(string rawMarkdown, bool allowHtmlTags = true, bool preventXSS = true);
}
