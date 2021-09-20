using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp.Settings;

namespace Volo.Abp.SettingManagement
{
    public static class TenantSettingManagerExtensions
    {
        public static Task<string> GetOrNullForTenantAsync(this ISettingManager settingManager, [NotNull] string name, Guid tenantId, bool fallback = true)
        {
            return settingManager.GetOrNullAsync(name, TenantSettingValueProvider.ProviderName, tenantId.ToString(), fallback);
        }

        public static Task<string> GetOrNullForCurrentTenantAsync(this ISettingManager settingManager, [NotNull] string name, bool fallback = true)
        {
            return settingManager.GetOrNullAsync(name, TenantSettingValueProvider.ProviderName, null, fallback);
        }

        public static Task<List<SettingValue>> GetAllForTenantAsync(this ISettingManager settingManager, Guid tenantId, bool fallback = true)
        {
            return settingManager.GetAllAsync(TenantSettingValueProvider.ProviderName, tenantId.ToString(), fallback);
        }

        public static Task<List<SettingValue>> GetAllForCurrentTenantAsync(this ISettingManager settingManager, bool fallback = true)
        {
            return settingManager.GetAllAsync(TenantSettingValueProvider.ProviderName, null, fallback);
        }

        public static Task SetForTenantAsync(this ISettingManager settingManager, Guid tenantId, [NotNull] string name, [CanBeNull] string value, bool forceToSet = false)
        {
            return settingManager.SetAsync(name, value, TenantSettingValueProvider.ProviderName, tenantId.ToString(), forceToSet);
        }

        public static Task SetForCurrentTenantAsync(this ISettingManager settingManager, [NotNull] string name, [CanBeNull] string value, bool forceToSet = false)
        {
            return settingManager.SetAsync(name, value, TenantSettingValueProvider.ProviderName, null, forceToSet);
        }

        public static Task SetForTenantOrGlobalAsync(this ISettingManager settingManager, Guid? tenantId, [NotNull] string name, [CanBeNull] string value, bool forceToSet = false)
        {
            if (tenantId.HasValue)
            {
                return settingManager.SetForTenantAsync(tenantId.Value, name, value, forceToSet);
            }

            return settingManager.SetGlobalAsync(name, value);
        }
    }
}
