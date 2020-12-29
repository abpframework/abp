using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.CmsKit.Public.Pages;
using Volo.CmsKit.Web.Pages;

namespace Volo.CmsKit.Public.Web.Pages.CmsKit.Pages
{
    public class IndexModel : CommonPageModel
    {
        [BindProperty(SupportsGet = true)] 
        public string PageUrl { get; set; }
        
        protected readonly IPageAppService PageAppService;

        public PageDto Page;
        
        public IndexModel(IPageAppService pageAppService)
        {
            PageAppService = pageAppService;
        }

        public async Task OnGetAsync()
        {
            Page = await PageAppService.GetByUrlAsync(PageUrl);
        }
    }
}