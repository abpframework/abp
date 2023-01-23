using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;
using Volo.CmsKit.Comments;
using Volo.CmsKit.Public.Comments;
using Volo.CmsKit.Public.Web.Security.Captcha;
using Volo.CmsKit.Web.Renderers;

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
    public CmsKitCommentOptions CmsKitCommentOptions { get; }
    public SimpleMathsCaptchaGenerator SimpleMathsCaptchaGenerator { get; }

    [HiddenInput]
    [BindProperty]
    public string RecaptchaToken { get; set; }

    [HiddenInput]
    [BindProperty]
    public Guid CaptchaId { get; set; }

    [BindProperty]
    public CommentingViewModel Input { get; set; }

    public CaptchaOutput CaptchaOutput { get; set; }

    public CommentingViewComponent(
        ICommentPublicAppService commentPublicAppService,
        IOptions<AbpMvcUiOptions> options,
        IMarkdownToHtmlRenderer markdownToHtmlRenderer,
        IOptions<CmsKitCommentOptions> cmsKitCommentOptions,
        SimpleMathsCaptchaGenerator simpleMathsCaptchaGenerator)
    {
        CommentPublicAppService = commentPublicAppService;
        MarkdownToHtmlRenderer = markdownToHtmlRenderer;
        AbpMvcUiOptions = options.Value;
        CmsKitCommentOptions = cmsKitCommentOptions.Value;
        SimpleMathsCaptchaGenerator = simpleMathsCaptchaGenerator;
    }

    public virtual async Task<IViewComponentResult> InvokeAsync(
        string entityType,
        string entityId,
        IEnumerable<string> referralLinks = null)
    {
        referralLinks ??= Enumerable.Empty<string>();
        var comments = (await CommentPublicAppService
            .GetListAsync(entityType, entityId)).Items;

        var loginUrl = $"{AbpMvcUiOptions.LoginUrl}?returnUrl={HttpContext.Request.Path.ToString()}&returnUrlHash=#cms-comment_{entityType}_{entityId}";

        var viewModel = new CommentingViewModel
        {
            EntityId = entityId,
            EntityType = entityType,
            ReferralLinks = referralLinks,
            LoginUrl = loginUrl,
            Comments = comments.OrderByDescending(i => i.CreationTime).ToList()
        };
        await ConvertMarkdownTextsToHtml(viewModel);

        if (CmsKitCommentOptions.IsRecaptchaEnabled)
        {
            CaptchaOutput = SimpleMathsCaptchaGenerator.Generate(new CaptchaOptions(
                    number1MinValue: 1,
                    number1MaxValue: 10,
                    number2MinValue: 5,
                    number2MaxValue: 15)
                );

            viewModel.CaptchaImageBase64 = GetCaptchaImageBase64(CaptchaOutput.ImageBytes);
        }
        this.Input = viewModel;
        return View("~/Pages/CmsKit/Shared/Components/Commenting/Default.cshtml", this);
    }

    private string GetCaptchaImageBase64(byte[] bytes)
    {
        return $"data:image/jpg;base64,{Convert.ToBase64String(bytes)}";
    }

    private async Task ConvertMarkdownTextsToHtml(CommentingViewModel viewModel)
    {
        viewModel.RawCommentTexts = new Dictionary<Guid, string>();
        var referralLinks = viewModel.ReferralLinks?.JoinAsString(" ");

        foreach (var comment in viewModel.Comments)
        {
            viewModel.RawCommentTexts.Add(comment.Id, comment.Text);
            comment.Text = await MarkdownToHtmlRenderer.RenderAsync(comment.Text, allowHtmlTags: false, preventXSS: true, referralLinks: referralLinks);

            foreach (var reply in comment.Replies)
            {
                viewModel.RawCommentTexts.Add(reply.Id, reply.Text);
                reply.Text = await MarkdownToHtmlRenderer.RenderAsync(reply.Text, allowHtmlTags: false, preventXSS: true);
            }
        }
    }

    public class CommentingViewModel
    {
        public string EntityType { get; set; }

        public string EntityId { get; set; }
        
        public IEnumerable<string> ReferralLinks { get; set; }

        public string LoginUrl { get; set; }

        public IReadOnlyList<CommentWithDetailsDto> Comments { get; set; }

        public Dictionary<Guid, string> RawCommentTexts { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string Captcha { get; set; }

        public string CaptchaImageBase64 { get; set; }
    }
}

