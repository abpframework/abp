using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Volo.CmsKit.Public.Pages
{
    public interface IPagePublicAppService
    {
        Task<PageDto> FindByUrlAsync([NotNull] string url);
    }
}