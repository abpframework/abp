using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Volo.CmsKit.Public.Pages
{
    public interface IPageAppService
    {
        Task<PageDto> GetByUrlAsync([NotNull] string url);
    }
}