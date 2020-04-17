using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Volo.Abp.TenantManagement.Web.Pages.TenantManagement.Tenants
{
    public class EditModalModel : TenantManagementPageModel
    {
        [BindProperty]
        public TenantInfoModel Tenant { get; set; }

        protected ITenantAppService TenantAppService { get; }

        public EditModalModel(ITenantAppService tenantAppService)
        {
            TenantAppService = tenantAppService;
        }

        public virtual async Task OnGetAsync(Guid id)
        {
            Tenant = ObjectMapper.Map<TenantDto, TenantInfoModel>(
                await TenantAppService.GetAsync(id)
            );
        }

        public virtual async Task<IActionResult> OnPostAsync()
        {
            ValidateModel();

            var input = ObjectMapper.Map<TenantInfoModel, TenantUpdateDto>(Tenant);
            await TenantAppService.UpdateAsync(Tenant.Id, input);

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