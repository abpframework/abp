using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyCompanyName.ProductManagement;
using ProductManagement;
using Volo.Abp.Application.Dtos;

namespace PublicWebSite.Host.Pages
{
    public class ProductsModel : PageModel
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