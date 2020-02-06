using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace Volo.Abp.TenantManagement.Web.Pages.TenantManagement.Tenants
{
    public class EditModalModel : TenantManagementPageModel
    {
        [BindProperty]
        public TenantInfoModel Tenant { get; set; }

        private readonly ITenantAppService _tenantAppService;

        public EditModalModel(ITenantAppService tenantAppService)
        {
            _tenantAppService = tenantAppService;
        }

        public async Task OnGetAsync(Guid id)
        {
            Tenant = ObjectMapper.Map<TenantDto, TenantInfoModel>(
                await _tenantAppService.GetAsync(id)
            );
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ValidateModel();

            var input = ObjectMapper.Map<TenantInfoModel, TenantUpdateDto>(Tenant);
            await _tenantAppService.UpdateAsync(Tenant.Id, input);

            return NoContent();
        }

        public class TenantInfoModel
        {
            [HiddenInput]
            public Guid Id { get; set; }

            [Required]
            [StringLength(TenantConsts.MaxNameLength)]
            [Display(Name = "DisplayName:TenantName")]
            public string Name { get; set; }
        }
    }
}