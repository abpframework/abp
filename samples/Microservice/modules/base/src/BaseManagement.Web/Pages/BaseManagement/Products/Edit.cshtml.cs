using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace BaseManagement.Pages.BaseManagement.Products
{
    public class EditModel : AbpPageModel
    {
        private readonly IBaseTypeAppService _productAppService;

        [BindProperty]
        public ProductEditViewModel Product { get; set; } = new ProductEditViewModel();

        public EditModel(IBaseTypeAppService productAppService)
        {
            _productAppService = productAppService;
        }

        public async Task<ActionResult> OnGetAsync(Guid productId)
        {
            var productDto = await _productAppService.GetAsync(productId);

            Product = ObjectMapper.Map<BaseTypeDto, ProductEditViewModel>(productDto);

            return Page();
        }

        public async Task OnPostAsync()
        {
            await _productAppService.UpdateAsync(Product.Id, new CreateUpdateBaseTypeDto()
            {
                Name = Product.Name,
            });
        }

        public class ProductEditViewModel
        {
            [HiddenInput]
            [Required]
            public Guid Id { get; set; }

            [Required]
            [StringLength(BaseConsts.MaxNameLength)]
            public string Name { get; set; }

      
            public float Price { get; set; }

            public int StockCount { get; set; }
        }
    }
}