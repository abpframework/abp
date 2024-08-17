using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.AspNetCore.MultiTenancy;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Volo.Abp.AspNetCore.Mvc.UI.MultiTenancy.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Abp.MultiTenancy;

namespace Pages.Abp.MultiTenancy;

public class TenantSwitchModalModel : AbpPageModel
{
    [BindProperty]
    public TenantInfoModel Input { get; set; } = default!;

    protected ITenantStore TenantStore { get; }
    protected ITenantNormalizer TenantNormalizer { get; }
    protected AbpAspNetCoreMultiTenancyOptions Options { get; }

    public TenantSwitchModalModel(
        ITenantStore tenantStore,
        ITenantNormalizer tenantNormalizer,
        IOptions<AbpAspNetCoreMultiTenancyOptions> options)
    {
        TenantStore = tenantStore;
        TenantNormalizer = tenantNormalizer;
        Options = options.Value;
        LocalizationResourceType = typeof(AbpUiMultiTenancyResource);
    }

    public virtual async Task OnGetAsync()
    {
        Input = new TenantInfoModel();

        if (CurrentTenant.IsAvailable)
        {
            var tenant = await TenantStore.FindAsync(CurrentTenant.GetId());
            Input.Name = tenant?.Name;
        }
    }

    public virtual async Task OnPostAsync()
    {
        Guid? tenantId = null;
        if (!Input.Name.IsNullOrEmpty())
        {
            var tenant = await TenantStore.FindAsync(TenantNormalizer.NormalizeName(Input.Name!)!);
            if (tenant == null)
            {
                throw new UserFriendlyException(L["GivenTenantIsNotExist", Input.Name!]);
            }

            if (!tenant.IsActive)
            {
                throw new UserFriendlyException(L["GivenTenantIsNotAvailable", Input.Name!]);
            }

            tenantId = tenant.Id;
        }

        AbpMultiTenancyCookieHelper.SetTenantCookie(HttpContext, tenantId, Options.TenantKey);
    }

    public class TenantInfoModel
    {
        [InputInfoText("SwitchTenantHint")]
        public string? Name { get; set; }
    }
}
