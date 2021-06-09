using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Volo.CmsKit.Menus
{
    public interface IMenuRepository : IBasicRepository<Menu, Guid>
    {
        Task<Menu> FindMainMenuAsync(bool includeDetails = true, CancellationToken cancellationToken = default);
    }
}
