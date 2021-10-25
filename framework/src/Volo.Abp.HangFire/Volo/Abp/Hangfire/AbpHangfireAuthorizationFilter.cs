using System;
using System.Threading.Tasks;
using Hangfire.Dashboard;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Users;

namespace Volo.Abp.Hangfire
{
    public class AbpHangfireAuthorizationFilter : IDashboardAsyncAuthorizationFilter
    {
        private readonly bool _enableTenant;
        private readonly string _requiredPermissionName;

        public AbpHangfireAuthorizationFilter(bool enableTenant = false, string requiredPermissionName = null)
        {
            _enableTenant = requiredPermissionName.IsNullOrWhiteSpace() ? enableTenant : true; 
            _requiredPermissionName = requiredPermissionName;
        }

        public async Task<bool> AuthorizeAsync(DashboardContext context)
        {
            if (!IsLoggedIn(context, _enableTenant))
            {
                return false;
            }

            if (_requiredPermissionName.IsNullOrEmpty())
            {
                return true;
            }

            return await IsPermissionGrantedAsync(context, _requiredPermissionName);
        }

        private static bool IsLoggedIn(DashboardContext context, bool enableTenant)
        {
            var currentUser = context.GetHttpContext().RequestServices.GetRequiredService<ICurrentUser>();

            if (!enableTenant)
            {
                return currentUser.IsAuthenticated && !currentUser.TenantId.HasValue;
            }
            
            return currentUser.IsAuthenticated;
        }

        private static async Task<bool> IsPermissionGrantedAsync(DashboardContext context, string requiredPermissionName)
        {
            var permissionChecker = context.GetHttpContext().RequestServices.GetRequiredService<IPermissionChecker>();
            return await permissionChecker.IsGrantedAsync(requiredPermissionName);
        }
    }
}