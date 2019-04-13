using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace Volo.Abp.TenantManagement.Web.Pages.TenantManagement.Tenants
{
    public class ConnectionStringsModal : AbpPageModel
    {
        [BindProperty]
        public TenantInfoModel Tenant { get; set; }

        private readonly ITenantAppService _tenantAppService;

        public ConnectionStringsModal(ITenantAppService tenantAppService)
        {
            _tenantAppService = tenantAppService;
        }

        public async Task OnGetAsync(Guid id)
        {
            var defaultConnectionString = await _tenantAppService.GetDefaultConnectionStringAsync(id);
            Tenant = new TenantInfoModel()
            {
                Id = id,
                DefaultConnectionString = defaultConnectionString,
                UseSharedDatabase = defaultConnectionString.IsNullOrWhiteSpace()
            };
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ValidateModel();

            if (Tenant.UseSharedDatabase || Tenant.DefaultConnectionString.IsNullOrWhiteSpace())
            {
                await _tenantAppService.RemoveDefaultConnectionStringAsync(Tenant.Id);

            }
            else
            {
                await _tenantAppService.SetDefaultConnectionStringAsync(Tenant.Id, Tenant.DefaultConnectionString);
            }

            return NoContent();
        }

        public class TenantInfoModel
        {
            [HiddenInput]
            public Guid Id { get; set; }

            [Display(Name = "DisplayName:UseSharedDatabase")]
            public bool UseSharedDatabase { get; set; }

            [StringLength(TenantConnectionStringConsts.MaxNameLength)]
            [Display(Name = "DisplayName:DefaultConnectionString")]
            public string DefaultConnectionString { get; set; }
        }
    }
}
