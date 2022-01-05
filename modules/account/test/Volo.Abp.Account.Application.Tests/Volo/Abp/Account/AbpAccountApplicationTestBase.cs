using System;
using System.Linq;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.Testing;

namespace Volo.Abp.Account;

public class AbpAccountApplicationTestBase : AbpIntegratedTest<AbpAccountApplicationTestModule>
{
    protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
    {
        options.UseAutofac();
    }

    protected virtual IdentityUser GetUser(string userName)
    {
        var user = UsingDbContext(context => context.Users.FirstOrDefault(u => u.UserName == userName));
        if (user == null)
        {
            throw new EntityNotFoundException();
        }

        return user;
    }

    protected virtual T UsingDbContext<T>(Func<IIdentityDbContext, T> action)
    {
        using (var dbContext = GetRequiredService<IIdentityDbContext>())
        {
            return action.Invoke(dbContext);
        }
    }
}
