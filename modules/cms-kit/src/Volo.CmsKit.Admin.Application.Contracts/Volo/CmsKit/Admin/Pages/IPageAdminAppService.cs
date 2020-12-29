using System;
using System.Threading.Tasks;

namespace Volo.CmsKit.Admin.Pages
{
    public interface IPageAdminAppService
    {
        Task<PageDto> GetAsync(Guid id);
        
        Task<PageDto> CreatePageAsync(CreatePageInputDto input);

        Task<PageDto> CreatePageWithContentAsync(CreatePageWithContentInputDto input);

        Task<PageDto> UpdatePageAsync(Guid id, UpdatePageInputDto input);

        Task<bool> DoesUrlExistAsync(CheckUrlInputDto input);
        
        Task UpdatePageContentAsync(Guid id, UpdatePageContentInputDto input);

        Task DeleteAsync(Guid id);
    }
}