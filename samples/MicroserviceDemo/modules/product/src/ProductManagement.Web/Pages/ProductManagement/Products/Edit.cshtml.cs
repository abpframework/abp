using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace ProductManagement.Pages.ProductManagement.Products
{
    public class EditModel : AbpPageModel
    {
        private readonly IProductAppService _productAppService;

        [BindProperty]
        public ProductEditModalView Product { get; set; } = new ProductEditModalView();

        public EditModel(IProductAppService productAppService)
        {
            _productAppService = productAppService;
        }

        public void OnGet()
        {
        }

        public async Task<ActionResult> OnGetAsync(Guid productId)
        {
            var productDto = await _productAppService.GetAsync(productId);

            Product = ObjectMapper.Map<ProductDto, ProductEditModalView>(productDto);

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

        public class ProductEditModalView
        {
            [HiddenInput]
            [Required]
            public Guid Id { get; set; }

            [Required]
            [StringLength(ProductConsts.MaxNameLength)]
            public string Name { get; set; }

            public float Price { get; set; }

            public int StockCount { get; set; }
        }
    }
}