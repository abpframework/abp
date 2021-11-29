using System;
using Volo.Abp.Domain.Repositories;

namespace Volo.CmsKit.Menus
{
    public interface IMenuItemRepository : IBasicRepository<MenuItem, Guid>
    {
    }
}
