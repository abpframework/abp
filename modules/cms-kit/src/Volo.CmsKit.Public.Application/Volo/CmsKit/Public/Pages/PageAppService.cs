using System.Threading.Tasks;
using Volo.CmsKit.Pages;

namespace Volo.CmsKit.Public.Pages
{
    public class PageAppService : CmsKitPublicAppServiceBase, IPageAppService
    {
        protected readonly IPageRepository PageRepository;

        public PageAppService(IPageRepository pageRepository)
        {
            PageRepository = pageRepository;
        }

        public virtual async Task<PageDto> GetByUrlAsync(string url)
        {
            var page = await PageRepository.GetByUrlAsync(url);
            
            return ObjectMapper.Map<Page, PageDto>(page);
        }
    }
}