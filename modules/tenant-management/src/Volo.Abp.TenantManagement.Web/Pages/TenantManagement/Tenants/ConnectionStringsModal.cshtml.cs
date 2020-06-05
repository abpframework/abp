using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Volo.Abp.TenantManagement.Web.Pages.TenantManagement.Tenants
{
    public class ConnectionStringsModal : TenantManagementPageModel
    {
        [BindProperty]
        public TenantInfoModel Tenant { get; set; }

        protected ITenantAppService TenantAppService { get; }

        public ConnectionStringsModal(ITenantAppService tenantAppService)
        {
            TenantAppService = tenantAppService;
        }

        public virtual async Task<IActionResult> OnGetAsync(Guid id)
        {
            var defaultConnectionString = await TenantAppService.GetDefaultConnectionStringAsync(id);
            Tenant = new TenantInfoModel
            {
                Id = id,
                DefaultConnectionString = defaultConnectionString,
                UseSharedDatabase = defaultConnectionString.IsNullOrWhiteSpace()
            };

            return Page();
        }

        public virtual async Task<IActionResult> OnPostAsync()
        {
            ValidateModel();

            if (Tenant.UseSharedDatabase || Tenant.DefaultConnectionString.IsNullOrWhiteSpace())
            {
                await TenantAppService.DeleteDefaultConnectionStringAsync(Tenant.Id);
            }
            else
            {
                await TenantAppService.UpdateDefaultConnectionStringAsync(Tenant.Id, Tenant.DefaultConnectionString);
            }

            return NoContent();
        }

        public class TenantInfoModel
        {
            [HiddenInput]
            public Guid Id { get; set; }

            public bool UseSharedDatabase { get; set; }

            [StringLength(TenantConnectionStringConsts.MaxValueLength)]
            public string DefaultConnectionString { get; set; }
        }
    }
}
