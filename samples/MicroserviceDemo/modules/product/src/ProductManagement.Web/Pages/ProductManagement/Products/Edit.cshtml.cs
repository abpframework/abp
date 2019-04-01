using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace ProductManagement.Pages.ProductManagement.Products
{
    public class EditModel : AbpPageModel
    {
        private readonly IProductAppService _productAppService;

        [BindProperty]
        public ProductEditViewModel Product { get; set; } = new ProductEditViewModel();

        public EditModel(IProductAppService productAppService)
        {
            _productAppService = productAppService;
        }

        public async Task<ActionResult> OnGetAsync(Guid productId)
        {
            var productDto = await _productAppService.GetAsync(productId);

            Product = ObjectMapper.Map<ProductDto, ProductEditViewModel>(productDto);

            return Page();
        }

        public async Task OnPostAsync()
        {
            await _productAppService.UpdateAsync(Product.Id, new UpdateProductDto()
            {
                Name = Product.Name,
                Price = Product.Price,
                StockCount = Product.StockCount
            });
        }

        public class ProductEditViewModel
        {
            [HiddenInput]
            [Required]
            public Guid Id { get; set; }

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