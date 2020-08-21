using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Validation;

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
            Tenant = new TenantInfoModel();
            return Task.FromResult<IActionResult>(Page());
        }

        public virtual async Task<IActionResult> OnPostAsync()
        {
            ValidateModel();

            var input = ObjectMapper.Map<TenantInfoModel, TenantCreateDto>(Tenant);
            await TenantAppService.CreateAsync(input);

            return NoContent();
        }

        public class TenantInfoModel: ExtensibleObject
        {
            [Required]
            [DynamicStringLength(typeof(TenantConsts), nameof(TenantConsts.MaxNameLength))]
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
