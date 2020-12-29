using System;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Volo.CmsKit.Pages
{
    public interface IPageAppService
    {
        Task<PageDto> GetAsync(Guid id);

        Task<PageDto> GetByUrlAsync([NotNull] string url);
        
        Task<PageDto> CreatePageAsync(CreatePageInputDto input);

        Task<PageDto> CreatePageWithContentAsync(CreatePageWithContentInputDto input);

        Task<PageDto> UpdatePageAsync(Guid id, UpdatePageInputDto input);

        Task<bool> DoesUrlExistAsync(CheckUrlInputDto input);
        
        Task UpdatePageContentAsync(Guid id, UpdatePageContentInputDto input);

        Task DeleteAsync(Guid id);
    }
}