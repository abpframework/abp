using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.CmsKit.Pages;

namespace Volo.CmsKit.Web.Pages.CmsKit.Pages
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