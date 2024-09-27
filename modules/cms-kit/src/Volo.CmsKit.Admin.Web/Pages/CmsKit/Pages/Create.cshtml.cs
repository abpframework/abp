using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Validation;
using Volo.CmsKit.Admin.Pages;
using Volo.CmsKit.Pages;

namespace Volo.CmsKit.Admin.Web.Pages.CmsKit.Pages;

public class CreateModel : CmsKitAdminPageModel
{
    protected readonly IPageAdminAppService pageAdminAppService;

    [BindProperty]
    public CreatePageViewModel ViewModel { get; set; }

    public CreateModel(IPageAdminAppService pageAdminAppService)
    {
        this.pageAdminAppService = pageAdminAppService;
        ViewModel = new CreatePageViewModel();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var createInput = ObjectMapper.Map<CreatePageViewModel, CreatePageInputDto>(ViewModel);

        var created = await pageAdminAppService.CreateAsync(createInput);

        return new OkObjectResult(created);
    }

    [AutoMap(typeof(CreatePageInputDto), ReverseMap = true)]
    public class CreatePageViewModel : ExtensibleObject
    {
        [Required]
        [DynamicMaxLength(typeof(PageConsts), nameof(PageConsts.MaxTitleLength))]
        public string Title { get; set; }

        [Required]
        [DynamicMaxLength(typeof(PageConsts), nameof(PageConsts.MaxLayoutNameLength))]
        public string LayoutName { get; set; }

        [Required]
        [DynamicMaxLength(typeof(PageConsts), nameof(PageConsts.MaxSlugLength))]
        public string Slug { get; set; }

        [HiddenInput]
        [DynamicMaxLength(typeof(PageConsts), nameof(PageConsts.MaxContentLength))]
        public string Content { get; set; }

        [TextArea(Rows = 6)]
        [DynamicMaxLength(typeof(PageConsts), nameof(PageConsts.MaxScriptLength))]
        public string Script { get; set; }

        [TextArea(Rows = 6)]
        [DynamicMaxLength(typeof(PageConsts), nameof(PageConsts.MaxStyleLength))]
        public string Style { get; set; }
    }
}
