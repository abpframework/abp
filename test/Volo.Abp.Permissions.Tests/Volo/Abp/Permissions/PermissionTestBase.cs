using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp.Permissions.EntityFrameworkCore;
using Volo.Abp.Users;

namespace Volo.Abp.Permissions
{
    public class PermissionTestBase : AbpIntegratedTest<AbpPermissionTestModule>
    {
        protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
        {
            options.UseAutofac();
        }

        protected virtual void UsingDbContext(Action<IAbpPermissionsDbContext> action)
        {
            using (var dbContext = GetRequiredService<IAbpPermissionsDbContext>())
            {
                action.Invoke(dbContext);
            }
        }

        protected virtual T UsingDbContext<T>(Func<IAbpPermissionsDbContext, T> action)
        {
            using (var dbContext = GetRequiredService<IAbpPermissionsDbContext>())
            {
                return action.Invoke(dbContext);
            }
        }

        protected List<PermissionGrant> GetSettingsFromDbContext(string entityType, string entityId, string name)
        {
            return UsingDbContext(context =>
                context.PermissionGrants.Where(
                    s =>
                        s.ProviderName == UserSettingValueProvider.ProviderName &&
                        s.ProviderKey == PermissionTestDataBuilder.User1Id.ToString() &&
                        s.Name == "MySetting2"
                ).ToList()
            );
        }
    }
}
