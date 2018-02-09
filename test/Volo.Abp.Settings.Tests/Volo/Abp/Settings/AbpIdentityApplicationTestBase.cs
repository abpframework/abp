using System;
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

    }
}
