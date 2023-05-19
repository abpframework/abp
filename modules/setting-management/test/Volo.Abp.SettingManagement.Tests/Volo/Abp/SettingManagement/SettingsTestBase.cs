using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp.SettingManagement.EntityFrameworkCore;

namespace Volo.Abp.SettingManagement;

public class SettingsTestBase : SettingManagementTestBase<AbpSettingManagementTestModule>
{
    protected virtual void UsingDbContext(Action<ISettingManagementDbContext> action)
    {
        using (var dbContext = GetRequiredService<ISettingManagementDbContext>())
        {
            action.Invoke(dbContext);
        }
    }

    protected virtual T UsingDbContext<T>(Func<ISettingManagementDbContext, T> action)
    {
        using (var dbContext = GetRequiredService<ISettingManagementDbContext>())
        {
            return action.Invoke(dbContext);
        }
    }

    protected List<Setting> GetSettingsFromDbContext(string entityType, string entityId, string name)
    {
        return UsingDbContext(context =>
            context.Settings.Where(
                s =>
                    s.ProviderName == entityType &&
                    s.ProviderKey == entityId &&
                    s.Name == name
            ).ToList()
        );
    }
}
