using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.GlobalFeatures;
using Volo.CmsKit.Admin.Menus;
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

        protected PageManager PageManager { get; }

        public PageAdminAppService(
            IPageRepository pageRepository,
            PageManager pageManager)
        {
            PageRepository = pageRepository;
            PageManager = pageManager;
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
            var page = await PageManager.CreateAsync(input.Title, input.Slug, input.Content, input.Script, input.Style);

            await PageRepository.InsertAsync(page);
            
            return ObjectMapper.Map<Page, PageDto>(page);
        }

        [Authorize(CmsKitAdminPermissions.Pages.Update)]
        public virtual async Task<PageDto> UpdateAsync(Guid id, UpdatePageInputDto input)
        {
            var page = await PageRepository.GetAsync(id);

            await PageManager.SetSlugAsync(page, input.Slug);

            page.SetTitle(input.Title);
            page.SetContent(input.Content);
            page.SetScript(input.Script);
            page.SetStyle(input.Style);

            await PageRepository.UpdateAsync(page);
            
            return ObjectMapper.Map<Page, PageDto>(page);
        }

        [Authorize(CmsKitAdminPermissions.Pages.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            await PageRepository.DeleteAsync(id);
        }
    }
}
