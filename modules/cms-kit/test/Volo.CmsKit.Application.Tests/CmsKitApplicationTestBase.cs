using System;
using Volo.CmsKit.EntityFrameworkCore;

namespace Volo.CmsKit
{
    /* Inherit from this class for your application layer tests.
     * See SampleAppService_Tests for example.
     */
    public abstract class CmsKitApplicationTestBase : CmsKitTestBase<CmsKitApplicationTestModule>
    {
        protected virtual void UsingDbContext(Action<ICmsKitDbContext> action)
        {
            using (var dbContext = GetRequiredService<ICmsKitDbContext>())
            {
                action.Invoke(dbContext);
            }
        }

        protected virtual T UsingDbContext<T>(Func<ICmsKitDbContext, T> action)
        {
            using (var dbContext = GetRequiredService<ICmsKitDbContext>())
            {
                return action.Invoke(dbContext);
            }
        }
    }
}
