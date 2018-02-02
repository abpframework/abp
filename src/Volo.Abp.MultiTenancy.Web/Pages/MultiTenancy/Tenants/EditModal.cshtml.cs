using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.RazorPages;

namespace Volo.Abp.MultiTenancy.Web.Pages.MultiTenancy.Tenants
{
    public class EditModalModel : AbpPageModel
    {
        [BindProperty]
        public TenantInfoModel TenantInfo { get; set; }

        private readonly ITenantAppService _tenantAppService;

        public EditModalModel(ITenantAppService tenantAppService)
        {
            _tenantAppService = tenantAppService;
        }

        public async Task OnGetAsync(Guid id)
        {
            TenantInfo = ObjectMapper.Map<TenantDto, TenantInfoModel>(
                await _tenantAppService.GetAsync(id)
            );
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ValidateModel();

            var input = ObjectMapper.Map<TenantInfoModel, TenantUpdateDto>(TenantInfo);
            await _tenantAppService.UpdateAsync(TenantInfo.Id, input);

            return NoContent();
        }

        public class TenantInfoModel
        {
            [HiddenInput]
            public Guid Id { get; set; }

            [Required]
            [StringLength(TenantConsts.MaxNameLength)]
            [Display(Name = "TenantName")]
            public string Name { get; set; }
        }
    }
}