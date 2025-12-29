using System;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Card;

public class AbpCardBodyTagHelperService : AbpTagHelperService<AbpCardBodyTagHelper>
{
    protected HtmlEncoder Encoder { get; }

    public AbpCardBodyTagHelperService(HtmlEncoder encoder)
    {
        Encoder = encoder;
    }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        output.Attributes.AddClass("card-body");

        ProcessTitle(output);
        ProcessSubtitle(output);
    }

    protected virtual void ProcessTitle(TagHelperOutput output)
    {
        if (!TagHelper.Title.IsNullOrWhiteSpace())
        {
            var cardTitle = new TagBuilder(AbpCardTitleTagHelper.DefaultHeading.ToHtmlTag());
            cardTitle.AddCssClass("card-title");
            cardTitle.InnerHtml.AppendHtml(Encoder.Encode(TagHelper.Title!));
            output.PreContent.AppendHtml(cardTitle);
        }
    }

    protected virtual void ProcessSubtitle(TagHelperOutput output)
    {
        if (!TagHelper.Subtitle.IsNullOrWhiteSpace())
        {
            var cardSubtitle = new TagBuilder(AbpCardSubtitleTagHelper.DefaultHeading.ToHtmlTag());
            cardSubtitle.AddCssClass("card-subtitle mb-2");
            cardSubtitle.InnerHtml.AppendHtml(Encoder.Encode(TagHelper.Subtitle!));
            output.PreContent.AppendHtml(cardSubtitle);
        }
    }
}
