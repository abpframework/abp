using System;
using System.Collections.Generic;
using System.Text;
using BaseManagement.EntityFrameworkCore;

namespace BaseManagement
{
    public abstract class BaseManagementApplicationTestBase : BaseManagementTestBase<BaseManagementApplicationTestModule>
    {
        protected virtual void UsingDbContext(Action<IBaseManagementDbContext> action)
        {
            using (var dbContext = GetRequiredService<IBaseManagementDbContext>())
            {
                action.Invoke(dbContext);
            }
        }

        protected virtual T UsingDbContext<T>(Func<IBaseManagementDbContext, T> action)
        {
            using (var dbContext = GetRequiredService<IBaseManagementDbContext>())
            {
                return action.Invoke(dbContext);
            }
        }
    }
}
