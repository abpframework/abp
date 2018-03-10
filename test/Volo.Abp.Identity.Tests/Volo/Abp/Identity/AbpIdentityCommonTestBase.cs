using System;
using System.Linq;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Volo.Abp.Identity
{
    public abstract class AbpIdentityCommonTestBase<TStartupModule> : AbpIntegratedTest<TStartupModule> 
        where TStartupModule : IAbpModule
    {
        protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
        {
            options.UseAutofac();
        }


        protected virtual IdentityUser GetUserAsync(string userName)
        {
            return UsingDbContext(context => context.Users.FirstOrDefault(u => u.UserName == userName));
        }

        protected virtual void UsingDbContext(Action<IIdentityDbContext> action)
        {
            using (var dbContext = GetRequiredService<IIdentityDbContext>())
            {
                action.Invoke(dbContext);
            }
        }

        protected virtual T UsingDbContext<T>(Func<IIdentityDbContext, T> action)
        {
            using (var dbContext = GetRequiredService<IIdentityDbContext>())
            {
                return action.Invoke(dbContext);
            }
        }
    }
}
