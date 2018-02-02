using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.RazorPages;

namespace Volo.Abp.MultiTenancy.Web.Pages.MultiTenancy.Tenants
{
    public class CreateModalModel : AbpPageModel
    {
        [BindProperty]
        public TenantInfoModel TenantModel { get; set; }

        private readonly ITenantAppService _tenantAppService;

        public CreateModalModel(ITenantAppService tenantAppService)
        {
            _tenantAppService = tenantAppService;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ValidateModel();

            var input = ObjectMapper.Map<TenantInfoModel, TenantCreateDto>(TenantModel);
            await _tenantAppService.CreateAsync(input);

            return NoContent();
        }

        public class TenantInfoModel
        {
            [Required]
            [StringLength(TenantConsts.MaxNameLength)]
            [Display(Name = "TenantName")]
            public string Name { get; set; }
        }
    }
}
