using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.AspNetCore.MultiTenancy;
using Volo.Abp.AspNetCore.Mvc.UI.MultiTenancy.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Abp.MultiTenancy;

namespace Pages.Abp.MultiTenancy
{
    public class TenantSwitchModalModel : AbpPageModel
    {
        [BindProperty]
        public TenantInfoModel Input { get; set; }

        protected ITenantStore TenantStore { get; }
        protected AbpAspNetCoreMultiTenancyOptions Options { get; }

        public TenantSwitchModalModel(
            ITenantStore tenantStore, 
            IOptions<AbpAspNetCoreMultiTenancyOptions> options)
        {
            TenantStore = tenantStore;
            Options = options.Value;
            LocalizationResourceType = typeof(AbpUiMultiTenancyResource);
        }

        public async Task OnGetAsync()
        {
            Input = new TenantInfoModel();

            if (CurrentTenant.IsAvailable)
            {
                var tenant = await TenantStore.FindAsync(CurrentTenant.GetId());
                Input.Name = tenant?.Name;
            }
        }

        public async Task OnPostAsync()
        {
            if (Input.Name.IsNullOrEmpty())
            {
                Response.Cookies.Delete(Options.TenantKey);
            }
            else
            {
                var tenant = await TenantStore.FindAsync(Input.Name);
                if (tenant == null)
                {
                    throw new UserFriendlyException(L["GivenTenantIsNotAvailable", Input.Name]);
                }

                Response.Cookies.Append(
                    Options.TenantKey,
                    tenant.Id.ToString(),
                    new CookieOptions
                    {
                        Path = "/",
                        HttpOnly = false,
                        Expires = DateTimeOffset.Now.AddYears(10)
                    }
                );
            }
        }

        public class TenantInfoModel
        {
            public string Name { get; set; }
        }
    }
}