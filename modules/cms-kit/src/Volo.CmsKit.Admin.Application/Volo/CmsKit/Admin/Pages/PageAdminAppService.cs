using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.GlobalFeatures;
using Volo.CmsKit.GlobalFeatures;
using Volo.CmsKit.Pages;
using Volo.CmsKit.Permissions;

namespace Volo.CmsKit.Admin.Pages
{
    [RequiresGlobalFeature(typeof(PagesFeature))]
    [Authorize(CmsKitAdminPermissions.Pages.Default)]
    public class PageAdminAppService : CmsKitAdminAppServiceBase, IPageAdminAppService
    {
        protected IPageRepository PageRepository { get; }
        
        public PageAdminAppService(IPageRepository pageRepository)
        {
            PageRepository = pageRepository;
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
            await CheckPageSlugAsync(input.Slug);

            var page = new Page(GuidGenerator.Create(), input.Title, input.Slug, input.Description, CurrentTenant?.Id);

            await PageRepository.InsertAsync(page);
            
            return ObjectMapper.Map<Page, PageDto>(page);
        }

        [Authorize(CmsKitAdminPermissions.Pages.Update)]
        public virtual async Task<PageDto> UpdateAsync(Guid id, UpdatePageInputDto input)
        {
            var page = await PageRepository.GetAsync(id);

            if (page.Slug != input.Slug)
            {
                await CheckPageSlugAsync(input.Slug);
            }

            page.SetTitle(input.Title);
            page.SetSlug(input.Slug);
            page.Description = input.Description;

            await PageRepository.UpdateAsync(page);
            
            return ObjectMapper.Map<Page, PageDto>(page);
        }

        [Authorize(CmsKitAdminPermissions.Pages.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            await PageRepository.DeleteAsync(id);
        }

        protected virtual async Task CheckPageSlugAsync(string slug)
        {
            if (await PageRepository.ExistsAsync(slug))
            {
                throw new PageSlugAlreadyExistsException(slug);
            }
        }
    }
}