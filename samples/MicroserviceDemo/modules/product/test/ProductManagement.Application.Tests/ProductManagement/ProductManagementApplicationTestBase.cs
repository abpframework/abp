using System;
using System.Collections.Generic;
using System.Text;
using ProductManagement.EntityFrameworkCore;

namespace ProductManagement
{
    public abstract class ProductManagementApplicationTestBase : ProductManagementTestBase<ProductManagementApplicationTestModule>
    {
        protected virtual void UsingDbContext(Action<IProductManagementDbContext> action)
        {
            using (var dbContext = GetRequiredService<IProductManagementDbContext>())
            {
                action.Invoke(dbContext);
            }
        }

        protected virtual T UsingDbContext<T>(Func<IProductManagementDbContext, T> action)
        {
            using (var dbContext = GetRequiredService<IProductManagementDbContext>())
            {
                return action.Invoke(dbContext);
            }
        }
    }
}
