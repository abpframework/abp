using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.Modularity;
using Volo.Abp.Uow;

namespace Volo.Abp.Identity;

public abstract class AbpIdentityExtendedTestBase<TStartupModule> : AbpIdentityTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    protected virtual IdentityUser GetUser(string userName)
    {
        var user = UsingDbContext(context => context.Users.FirstOrDefault(u => u.UserName == userName));
        if (user == null)
        {
            throw new EntityNotFoundException();
        }

        return user;
    }

    protected virtual IdentityUser FindUser(string userName)
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

    protected virtual async Task UsingUowAsync(Func<Task> action)
    {
        using (var uow = GetRequiredService<IUnitOfWorkManager>().Begin())
        {
            await action();
            await uow.CompleteAsync();
        }
    }
}
