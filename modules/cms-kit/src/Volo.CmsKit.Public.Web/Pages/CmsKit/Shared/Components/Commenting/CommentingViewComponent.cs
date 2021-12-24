using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;
using Volo.CmsKit.Public.Comments;
using Volo.CmsKit.Public.Web.Renderers;

namespace Volo.CmsKit.Public.Web.Pages.CmsKit.Shared.Components.Commenting;

[ViewComponent(Name = "CmsCommenting")]
[Widget(
    ScriptTypes = new[] { typeof(CommentingScriptBundleContributor) },
    StyleTypes = new[] { typeof(CommentingStyleBundleContributor) },
    RefreshUrl = "/CmsKitPublicWidgets/Commenting",
    AutoInitialize = true
)]
public class CommentingViewComponent : AbpViewComponent
{
    public ICommentPublicAppService CommentPublicAppService { get; }
    public IMarkdownToHtmlRenderer MarkdownToHtmlRenderer { get; }
    public AbpMvcUiOptions AbpMvcUiOptions { get; }

    public CommentingViewComponent(
        ICommentPublicAppService commentPublicAppService,
        IOptions<AbpMvcUiOptions> options,
        IMarkdownToHtmlRenderer markdownToHtmlRenderer)
    {
        CommentPublicAppService = commentPublicAppService;
        MarkdownToHtmlRenderer = markdownToHtmlRenderer;
        AbpMvcUiOptions = options.Value;
    }

    public virtual async Task<IViewComponentResult> InvokeAsync(
        string entityType,
        string entityId)
    {
        var comments = (await CommentPublicAppService
            .GetListAsync(entityType, entityId)).Items;


        var loginUrl = $"{AbpMvcUiOptions.LoginUrl}?returnUrl={HttpContext.Request.Path.ToString()}&returnUrlHash=#cms-comment_{entityType}_{entityId}";

        var viewModel = new CommentingViewModel
        {
            EntityId = entityId,
            EntityType = entityType,
            LoginUrl = loginUrl,
            Comments = comments.OrderByDescending(i => i.CreationTime).ToList()
        };

        await ConvertMarkdownTextsToHtml(viewModel);

        return View("~/Pages/CmsKit/Shared/Components/Commenting/Default.cshtml", viewModel);
    }

    private async Task ConvertMarkdownTextsToHtml(CommentingViewModel viewModel)
    {
        viewModel.RawCommentTexts = new Dictionary<Guid, string>();

        foreach (var comment in viewModel.Comments)
        {
            viewModel.RawCommentTexts.Add(comment.Id, comment.Text);
            comment.Text = await MarkdownToHtmlRenderer.RenderAsync(comment.Text, true);

            foreach (var reply in comment.Replies)
            {
                viewModel.RawCommentTexts.Add(reply.Id, reply.Text);
                reply.Text = await MarkdownToHtmlRenderer.RenderAsync(reply.Text, true);
            }
        }
    }

    public class CommentingViewModel
    {
        public string EntityType { get; set; }

        public string EntityId { get; set; }

        public string LoginUrl { get; set; }

        public IReadOnlyList<CommentWithDetailsDto> Comments { get; set; }

        public Dictionary<Guid, string> RawCommentTexts { get; set; }
    }
}

