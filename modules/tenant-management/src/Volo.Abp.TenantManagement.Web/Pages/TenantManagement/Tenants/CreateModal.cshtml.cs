using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace Volo.Abp.TenantManagement.Web.Pages.TenantManagement.Tenants
{
    public class CreateModalModel : TenantManagementPageModel
    {
        [BindProperty]
        public TenantInfoModel Tenant { get; set; }

        private readonly ITenantAppService _tenantAppService;

        public CreateModalModel(ITenantAppService tenantAppService)
        {
            _tenantAppService = tenantAppService;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ValidateModel();

            var input = ObjectMapper.Map<TenantInfoModel, TenantCreateDto>(Tenant);
            await _tenantAppService.CreateAsync(input);

            return NoContent();
        }

        public class TenantInfoModel
        {
            [Required]
            [StringLength(TenantConsts.MaxNameLength)]
            public string Name { get; set; }

            [Required]
            [EmailAddress]
            [MaxLength(256)]
            public string AdminEmailAddress { get; set; }

            [Required]
            [MaxLength(128)]
            public string AdminPassword { get; set; }
        }
    }
}
