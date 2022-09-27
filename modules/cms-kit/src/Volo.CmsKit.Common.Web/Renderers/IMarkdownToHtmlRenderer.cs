using System.Threading.Tasks;

namespace Volo.CmsKit.Web.Renderers;

public interface IMarkdownToHtmlRenderer
{
    Task<string> RenderAsync(string rawMarkdown, bool allowHtmlTags = true, bool preventXSS = true);
}
