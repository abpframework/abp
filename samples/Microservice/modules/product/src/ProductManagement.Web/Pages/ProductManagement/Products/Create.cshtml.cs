using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace ProductManagement.Pages.ProductManagement.Products
{
    public class CreateModel : AbpPageModel
    {
        private readonly IProductAppService _productAppService;

        [BindProperty]
        public ProductCreateViewModel Product { get; set; } = new ProductCreateViewModel();

        public CreateModel(IProductAppService productAppService)
        {
            _productAppService = productAppService;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var createProductDto = ObjectMapper.Map<ProductCreateViewModel, CreateProductDto>(Product);

            await _productAppService.CreateAsync(createProductDto);

            return NoContent();
        }

        public class ProductCreateViewModel
        {
            [Required]
            [StringLength(ProductConsts.MaxCodeLength)]
            public string Code { get; set; }

            [Required]
            [StringLength(ProductConsts.MaxNameLength)]
            public string Name { get; set; }

            [StringLength(ProductConsts.MaxImageNameLength)]
            public string ImageName { get; set; }

            public float Price { get; set; }

            public int StockCount { get; set; }
        }
    }
}