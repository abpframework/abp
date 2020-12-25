using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Volo.CmsKit.Pages
{
    public interface IPageRepository : IBasicRepository<Page, Guid>
    {
        Task<Page> GetByUrlAsync(string url);

        Task<Page> FindByUrlAsync(string url);

        Task<bool> DoesExistAsync(string url);
    }
}