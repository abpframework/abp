using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace BaseManagement.Pages.BaseManagement.Products
{
    public class CreateModel : AbpPageModel
    {
        private readonly IBaseTypeAppService _productAppService;

        [BindProperty]
        public ProductCreateViewModel Product { get; set; } = new ProductCreateViewModel();

        public CreateModel(IBaseTypeAppService productAppService)
        {
            _productAppService = productAppService;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var createProductDto = ObjectMapper.Map<ProductCreateViewModel, CreateBaseTypeDto>(Product);

            await _productAppService.CreateAsync(createProductDto);

            return NoContent();
        }

        public class ProductCreateViewModel
        {
            [Required]
            [StringLength(BaseConsts.MaxCodeLength)]
            public string Code { get; set; }

            [Required]
            [StringLength(BaseConsts.MaxNameLength)]
            public string Name { get; set; }

        }
    }
}