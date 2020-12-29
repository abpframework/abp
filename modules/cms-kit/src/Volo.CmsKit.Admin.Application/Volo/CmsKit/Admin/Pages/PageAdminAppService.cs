using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.CmsKit.Contents;
using Volo.CmsKit.Pages;

namespace Volo.CmsKit.Admin.Pages
{
    public class PageAdminAppService : CmsKitAdminAppServiceBase, IPageAdminAppService
    {
        protected readonly IPageRepository PageRepository;
        protected readonly IContentRepository ContentRepository;

        public PageAdminAppService(IPageRepository pageRepository, IContentRepository contentRepository)
        {
            PageRepository = pageRepository;
            ContentRepository = contentRepository;
        }

        public virtual async Task<PageDto> GetAsync(Guid id)
        {
            var page = await PageRepository.GetAsync(id);

            return ObjectMapper.Map<Page, PageDto>(page);
        }

        public virtual async Task<PageDto> CreatePageAsync(CreatePageInputDto input)
        {
            var page = await CreatePageAsync(input.Title, input.Url, input.Description);

            await PageRepository.InsertAsync(page);
            
            return ObjectMapper.Map<Page, PageDto>(page);
        }

        public virtual async Task<PageDto> CreatePageWithContentAsync(CreatePageWithContentInputDto input)
        {
            var page = await CreatePageAsync(input.Title, input.Url, input.Description);

            await PageRepository.InsertAsync(page);
            
            var content = new Content(
                GuidGenerator.Create(),
                nameof(Page),
                page.Id.ToString(),
                input.Content,
                page.TenantId);

            await ContentRepository.InsertAsync(content);
            
            return ObjectMapper.Map<Page, PageDto>(page);
        }

        public virtual async Task<PageDto> UpdatePageAsync(Guid id, UpdatePageInputDto input)
        {
            var page = await PageRepository.GetAsync(id);

            if (page.Url != input.Url)
            {
                await CheckPageUrlAsync(input.Url);
            }

            page.Title = input.Title;
            page.Url = input.Url;
            page.Description = input.Description;

            await PageRepository.UpdateAsync(page);

            return ObjectMapper.Map<Page, PageDto>(page);
        }

        public virtual Task<bool> DoesUrlExistAsync(CheckUrlInputDto input)
        {
            return PageRepository.DoesExistAsync(input.Url);
        }

        public virtual async Task UpdatePageContentAsync(Guid id, UpdatePageContentInputDto input)
        {
            var pageContent = await ContentRepository.GetAsync(nameof(Page), id.ToString());
            
            pageContent.SetValue(input.Content);

            await ContentRepository.UpdateAsync(pageContent);
        }

        public virtual async Task DeleteAsync(Guid id)
        {
            await ContentRepository.DeleteAsync(nameof(Page), id.ToString(), CurrentTenant?.Id, CancellationToken.None);
            await PageRepository.DeleteAsync(id, cancellationToken: CancellationToken.None);
        }

        protected virtual async Task<Page> CreatePageAsync(string title, string url, string description)
        {
            await CheckPageUrlAsync(url);

            return new Page(GuidGenerator.Create(), title, url, description, CurrentTenant?.Id);
        }

        protected virtual async Task CheckPageUrlAsync(string url)
        {
            if (await PageRepository.DoesExistAsync(url))
            {
                throw new UserFriendlyException("Url exist");
            }
        }
    }
}