using System;
using Volo.Abp.MultiTenancy.EntityFrameworkCore;
using Volo.Abp.TestBase;

namespace Volo.Abp.MultiTenancy
{
    public class AbpMultiTenancyApplicationTestBase : AbpIntegratedTest<AbpMultiTenancyApplicationTestModule>
    {
        protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
        {
            options.UseAutofac();
        }

        protected virtual void UsingDbContext(Action<IMultiTenancyDbContext> action)
        {
            using (var dbContext = GetRequiredService<IMultiTenancyDbContext>())
            {
                action.Invoke(dbContext);
            }
        }

        protected virtual T UsingDbContext<T>(Func<IMultiTenancyDbContext, T> action)
        {
            using (var dbContext = GetRequiredService<IMultiTenancyDbContext>())
            {
                return action.Invoke(dbContext);
            }
        }
    }
}
