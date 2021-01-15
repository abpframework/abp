using System.Threading.Tasks;

namespace Volo.CmsKit.Web.Contents
{
    public interface IContentRenderer
    {
        Task<string> RenderAsync(string value);
    }
}