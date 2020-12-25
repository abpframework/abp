using System;
using System.Threading.Tasks;

namespace Volo.CmsKit.Pages
{
    public interface IPageAppService
    {
        Task<PageDto> CreatePageAsync(CreatePageInputDto input);

        Task<PageDto> CreatePageWithContentAsync(CreatePageWithContentInputDto input);

        Task<PageDto> UpdatePageAsync(Guid id, UpdatePageInputDto input);

        Task<bool> DoesUrlExistAsync(CheckUrlInputDto input);
        
        Task UpdatePageContentAsync(Guid id, UpdatePageContentInputDto input);

        Task DeletePageAsync(Guid id);
    }
}