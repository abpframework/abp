using System.Threading.Tasks;
using MyCompanyName.ProductManagement;
using ProductManagement;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace PublicWebSite.Host.Pages
{
    public class ProductsModel : AbpPageModel
    {
        public ListResultDto<ProductDto> Products { get; set; }

        private readonly IPublicProductAppService _productAppService;

        public ProductsModel(IPublicProductAppService productAppService)
        {
            _productAppService = productAppService;
        }

        public async Task OnGetAsync()
        {
            Products = await _productAppService.GetListAsync();
        }
    }
}