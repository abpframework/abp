using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Volo.CmsKit.Public.Pages
{
    public interface IPagePublicAppService
    {
        Task<PageDto> FindBySlugAsync([NotNull] string slug);
    }
}