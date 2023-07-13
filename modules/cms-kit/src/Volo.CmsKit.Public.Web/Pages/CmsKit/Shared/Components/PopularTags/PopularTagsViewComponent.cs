using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Uow;
using Volo.CmsKit.Tags;

namespace Volo.CmsKit.Public.Web.Pages.CmsKit.Shared.Components.PopularTags;

public class PopularTagsViewComponent : AbpViewComponent
{
    private readonly ITagAppService _tagAppService;
    
    public PopularTagsViewComponent(ITagAppService tagAppService)
    {
        _tagAppService = tagAppService;
    }
    
    public async Task<IViewComponentResult> InvokeAsync(string entityType, int maxCount, Func<PopularTagDto, string> urlFactory = null)
    {
        var model = new PopularTagsViewModel
        {
            Tags = await _tagAppService.GetPopularTagsAsync(entityType, maxCount),
            UrlFactory = urlFactory
        };
        return View("~/Pages/CmsKit/Shared/Components/PopularTags/Default.cshtml", model);
    }

    public class PopularTagsViewModel
    {
        public List<PopularTagDto> Tags { get; set; }
        public Func<PopularTagDto, string> UrlFactory { get; set; }
    }
}