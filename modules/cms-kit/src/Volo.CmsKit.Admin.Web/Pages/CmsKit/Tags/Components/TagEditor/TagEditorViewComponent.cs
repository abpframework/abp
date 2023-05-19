using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;
using Volo.CmsKit.Tags;

namespace Volo.CmsKit.Admin.Web.Pages.CmsKit.Tags.Components.TagEditor;

[Widget(
    ScriptFiles = new[]
    {
            "/Pages/CmsKit/Tags/Components/TagEditor/default.js"
    })]
public class TagEditorViewComponent : AbpViewComponent
{
    protected ITagAppService TagAppService { get; }

    public TagEditorViewComponent(ITagAppService tagAppService)
    {
        TagAppService = tagAppService;
    }

    public virtual async Task<IViewComponentResult> InvokeAsync(string entityType, string entityId, bool displaySubmitButton = true)
    {
        var tags =
            entityId.IsNullOrWhiteSpace() ?
            new List<TagDto>() :
            await TagAppService.GetAllRelatedTagsAsync(entityType, entityId);

        return View("~/Pages/CmsKit/Tags/Components/TagEditor/Default.cshtml", new TagEditorViewModel
        {
            EntityId = entityId,
            EntityType = entityType,
            Tags = tags,
            DisplaySubmitButton = displaySubmitButton
        });
    }

    public class TagEditorViewModel
    {
        public string EntityType { get; set; }
        public string EntityId { get; set; }
        public List<TagDto> Tags { get; set; }
        public bool DisplaySubmitButton { get; set; }
    }
}
