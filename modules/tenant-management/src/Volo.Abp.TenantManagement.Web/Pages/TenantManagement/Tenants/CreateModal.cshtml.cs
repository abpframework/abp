using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Volo.Abp.TenantManagement.Web.Pages.TenantManagement.Tenants
{
    public class CreateModalModel : TenantManagementPageModel
    {
        [BindProperty]
        public TenantInfoModel Tenant { get; set; }

        protected ITenantAppService TenantAppService { get; }

        public CreateModalModel(ITenantAppService tenantAppService)
        {
            TenantAppService = tenantAppService;
        }

        public virtual Task<IActionResult> OnGetAsync()
        {
            return Task.FromResult<IActionResult>(Page());
        }

        public virtual async Task<IActionResult> OnPostAsync()
        {
            ValidateModel();

            var input = ObjectMapper.Map<TenantInfoModel, TenantCreateDto>(Tenant);
            await TenantAppService.CreateAsync(input);

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
            [DataType(DataType.Password)]
            [MaxLength(128)]
            public string AdminPassword { get; set; }
        }
    }
}
