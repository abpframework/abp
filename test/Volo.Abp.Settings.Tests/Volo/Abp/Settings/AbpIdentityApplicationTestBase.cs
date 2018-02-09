using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp.Session;
using Volo.Abp.Settings.EntityFrameworkCore;
using Volo.Abp.TestBase;

namespace Volo.Abp.Settings
{
    public class AbpSettingsTestBase : AbpIntegratedTest<AbpSettingsTestModule>
    {
        protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
        {
            options.UseAutofac();
        }

        protected virtual void UsingDbContext(Action<IAbpSettingsDbContext> action)
        {
            using (var dbContext = GetRequiredService<IAbpSettingsDbContext>())
            {
                action.Invoke(dbContext);
            }
        }

        protected virtual T UsingDbContext<T>(Func<IAbpSettingsDbContext, T> action)
        {
            using (var dbContext = GetRequiredService<IAbpSettingsDbContext>())
            {
                return action.Invoke(dbContext);
            }
        }

        protected List<Setting> GetSettingsFromDbContext(string entityType, string entityId, string name)
        {
            return UsingDbContext(context =>
                context.Settings.Where(
                    s =>
                        s.EntityType == UserSettingValueProvider.DefaultEntityType &&
                        s.EntityId == AbpIdentityTestDataBuilder.User1Id.ToString() &&
                        s.Name == "MySetting2"
                ).ToList()
            );
        }
    }
}
