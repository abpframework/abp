using System;
using Volo.Abp.Domain.Repositories;

namespace Volo.CmsKit.Pages
{
    public interface IPageRepository : IBasicRepository<Page, Guid>
    {
        
    }
}