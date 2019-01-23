using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProductManagement.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using System.Linq;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;

namespace ProductManagement
{
    public class EfCoreProductRepository : EfCoreRepository<IProductManagementDbContext, Product, Guid>, IProductRepository
    {
        public EfCoreProductRepository(IDbContextProvider<IProductManagementDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<List<Product>> GetListAsync(string sorting, int maxResultCount, int skipCount)
        {
            // TODO: refactor sorting
            var products = await DbSet.OrderBy(sorting ?? "creationTime desc")
                .PageBy(skipCount, maxResultCount)
                .ToListAsync();

            return products;
        }
    }
}
