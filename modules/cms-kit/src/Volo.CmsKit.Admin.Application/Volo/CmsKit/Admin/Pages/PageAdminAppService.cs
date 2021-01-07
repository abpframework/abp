using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.BlobStoring;
using Volo.Abp.Content;
using Volo.Abp.GlobalFeatures;
using Volo.CmsKit.Contents;
using Volo.CmsKit.GlobalFeatures;
using Volo.CmsKit.Pages;
using Volo.CmsKit.Permissions;

namespace Volo.CmsKit.Admin.Pages
{
    [RequiresGlobalFeature(typeof(PagesFeature))]
    [Authorize(CmsKitAdminPermissions.Pages.Default)]
    public class PageAdminAppService : CmsKitAdminAppServiceBase, IPageAdminAppService
    {
        protected readonly IPageRepository PageRepository;
        protected readonly IContentRepository ContentRepository;

        protected readonly IBlobContainer<PageImageContainer> BlobContainer;
        
        public PageAdminAppService(
            IPageRepository pageRepository,
            IContentRepository contentRepository,
            IBlobContainer<PageImageContainer> blobContainer)
        {
            PageRepository = pageRepository;
            ContentRepository = contentRepository;
            BlobContainer = blobContainer;
        }

        public virtual async Task<PageDto> GetAsync(Guid id)
        {
            var page = await PageRepository.GetAsync(id);

            return ObjectMapper.Map<Page, PageDto>(page);
        }

        public virtual async Task<PagedResultDto<PageDto>> GetListAsync(GetPagesInputDto input)
        {
            var count = await PageRepository.GetCountAsync(input.Filter);
            
            var pages = await PageRepository.GetListAsync(
                input.Filter,
                input.MaxResultCount,
                input.SkipCount,
                input.Sorting
            );

            return new PagedResultDto<PageDto>(
                count,
                ObjectMapper.Map<List<Page>, List<PageDto>>(pages)
            );
        }

        [Authorize(CmsKitAdminPermissions.Pages.Create)]
        public virtual async Task<PageDto> CreateAsync(CreatePageInputDto input)
        {
            await CheckPageUrlAsync(input.Url);

            var page = new Page(GuidGenerator.Create(), input.Title, input.Url, input.Description, CurrentTenant?.Id);

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

        [Authorize(CmsKitAdminPermissions.Pages.Update)]
        public virtual async Task<PageDto> UpdateAsync(Guid id, UpdatePageInputDto input)
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

            var content = await ContentRepository.GetAsync(nameof(Page), page.Id.ToString());

            content.SetValue(input.Content);

            await ContentRepository.UpdateAsync(content);
            
            return ObjectMapper.Map<Page, PageDto>(page);
        }

        [Authorize(CmsKitAdminPermissions.Pages.Update)]
        public virtual async Task SetImageAsync(Guid id, RemoteStreamContent content)
        {
            var page = await PageRepository.GetAsync(id);

            await BlobContainer.SaveAsync(page.Id.ToString(), content.GetStream());
        }

        public virtual async Task<RemoteStreamContent> GetImageAsync(Guid id)
        {
            var blobStream = await BlobContainer.GetAsync(id.ToString());
            
            return new RemoteStreamContent(blobStream);
        }

        [Authorize(CmsKitAdminPermissions.Pages.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            await ContentRepository.DeleteAsync(nameof(Page), id.ToString(), CurrentTenant?.Id, CancellationToken.None);
            await PageRepository.DeleteAsync(id, cancellationToken: CancellationToken.None);
        }

        protected virtual async Task CheckPageUrlAsync(string url)
        {
            if (await PageRepository.ExistsAsync(url))
            {
                throw new PageUrlAlreadyExistException(url);
            }
        }
    }
}