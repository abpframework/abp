using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Volo.CmsKit.Menus;

public interface IMenuItemRepository : IBasicRepository<MenuItem, Guid>
{
    Task<List<MenuItem>> GetOrderedListAsync(bool includeDetails = false, CancellationToken cancellationToken = default);

    Task<int> GetHighestMenuOrderAsync(Guid? parentId = null, CancellationToken cancellationToken = default);
}
