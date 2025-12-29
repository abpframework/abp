using System.Text.Encodings.Web;
using Localization.Resources.AbpUi;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Modal;

public class AbpModalHeaderTagHelperService : AbpTagHelperService<AbpModalHeaderTagHelper>
{
    protected IStringLocalizer<AbpUiResource> L { get; }
    protected HtmlEncoder Encoder { get; }

    public AbpModalHeaderTagHelperService(IStringLocalizer<AbpUiResource> localizer, HtmlEncoder encoder)
    {
        L = localizer;
        Encoder = encoder;
    }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        output.Attributes.AddClass("modal-header");
        output.PreContent.SetHtmlContent(CreatePreContent());
        output.PostContent.SetHtmlContent(CreatePostContent());
    }

    protected virtual string CreatePreContent()
    {
        var title = new TagBuilder("h5");
        title.AddCssClass("modal-title");
        title.InnerHtml.AppendHtml(Encoder.Encode(TagHelper.Title));

        return title.ToHtmlString();
    }

    protected virtual string CreatePostContent()
    {
        var button = new TagBuilder("button");
        button.AddCssClass("btn-close");
        button.Attributes.Add("type", "button");
        button.Attributes.Add("data-bs-dismiss", "modal");
        button.Attributes.Add("aria-label", L["Close"].Value);

        return button.ToHtmlString();
    }
}
