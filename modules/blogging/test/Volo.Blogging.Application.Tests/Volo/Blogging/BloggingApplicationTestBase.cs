using System;
using Volo.Blogging.EntityFrameworkCore;

namespace Volo.Blogging
{
    public abstract class BloggingApplicationTestBase : BloggingTestBase<BloggingApplicationTestModule>
    {
        protected virtual void UsingDbContext(Action<IBloggingDbContext> action)
        {
            using (var dbContext = GetRequiredService<IBloggingDbContext>())
            {
                action.Invoke(dbContext);
            }
        }

        protected virtual T UsingDbContext<T>(Func<IBloggingDbContext, T> action)
        {
            using (var dbContext = GetRequiredService<IBloggingDbContext>())
            {
                return action.Invoke(dbContext);
            }
        }
    }
}