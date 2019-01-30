using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace ProductManagement
{
    public interface IProductRepository : IBasicRepository<Product, Guid>
    {
        Task<List<Product>> GetListAsync(string sorting, int maxResultCount, int skipCount);
    }
}
