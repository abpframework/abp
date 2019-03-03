using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Users;

namespace Volo.Abp.AspNetCore.Mvc.UI.MultiTenancy.Components.TenantSwitch
{
    public class TenantSwitchViewComponent : AbpViewComponent
    {
        /// <summary>
        /// -1,000,000
        /// </summary>
        public const int Order = -1_000_000;

        protected ITenantStore TenantStore { get; }
        protected ICurrentTenant CurrentTenant { get; }
        protected ICurrentUser CurrentUser { get; }

        public TenantSwitchViewComponent(
            ITenantStore tenantStore, 
            ICurrentTenant currentTenant,
            ICurrentUser currentUser)
        {
            TenantStore = tenantStore;
            CurrentTenant = currentTenant;
            CurrentUser = currentUser;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = new TenantSwitchViewModel
            {
                CurrentUser = CurrentUser
            };

            if (CurrentTenant.Id.HasValue)
            {
                model.Tenant = await TenantStore.FindAsync(CurrentTenant.GetId());
            }

            return View("~/Volo/Abp/AspNetCore/Mvc/UI/MultiTenancy/Components/TenantSwitch/Default.cshtml", model);
        }

        public class TenantSwitchViewModel
        {
            public TenantInfo Tenant { get; set; }

            public ICurrentUser CurrentUser { get; set; }
        }
    }
}
