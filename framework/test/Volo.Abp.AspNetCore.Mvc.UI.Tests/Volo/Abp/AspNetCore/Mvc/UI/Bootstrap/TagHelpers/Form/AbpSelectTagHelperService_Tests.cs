#nullable enable

using System.Collections.Generic;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Localization;
using Shouldly;
using Xunit;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

public class AbpSelectTagHelperService_Tests
{
    [Fact]
    public async Task Info_text_should_be_rendered_as_div_with_form_text_class()
    {
        var service = new TestAbpSelectTagHelperService();
        var tagHelper = new AbpSelectTagHelper(service)
        {
            AspFor = CreateModelExpression(),
            InfoText = "Description"
        };

        var output = CreateOutput();

        await tagHelper.ProcessAsync(CreateContext(), output);

        service.LastGroupHtml.ShouldContain("<div class=\"form-text\"");
        service.LastGroupHtml.ShouldContain("id=\"TestSelectInfoText\"");
        service.LastGroupHtml.ShouldNotContain("<small");
    }

    [Fact]
    public async Task Info_text_should_set_aria_describedby_on_select()
    {
        var service = new TestAbpSelectTagHelperService();
        var tagHelper = new AbpSelectTagHelper(service)
        {
            AspFor = CreateModelExpression(),
            InfoText = "Description"
        };

        var output = CreateOutput();

        await tagHelper.ProcessAsync(CreateContext(), output);

        service.LastSelectTag.ShouldNotBeNull();
        service.LastSelectTag!.Attributes.ContainsName("aria-describedby").ShouldBeTrue();
        service.LastSelectTag.Attributes["aria-describedby"].Value.ToString().ShouldBe("TestSelectInfoText");
    }

    private static TagHelperContext CreateContext()
    {
        return new TagHelperContext(
            new TagHelperAttributeList(),
            new Dictionary<object, object>(),
            "test");
    }

    private static TagHelperOutput CreateOutput()
    {
        return new TagHelperOutput(
            "abp-select",
            new TagHelperAttributeList(),
            (_, _) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));
    }

    private static ModelExpression CreateModelExpression()
    {
        var metadataProvider = new EmptyModelMetadataProvider();
        return new ModelExpression(
            "TestSelect",
            metadataProvider.GetModelExplorerForType(typeof(string), null));
    }

    private sealed class TestAbpSelectTagHelperService : AbpSelectTagHelperService
    {
        public string LastGroupHtml { get; private set; } = string.Empty;

        public TagHelperOutput? LastSelectTag { get; private set; }

        public TestAbpSelectTagHelperService()
            : base(null!, HtmlEncoder.Default, new FakeTagHelperLocalizer(), null!, null!)
        {
        }

        protected override Task<TagHelperOutput> GetSelectTagAsync(TagHelperContext context, TagHelperOutput output, TagHelperContent childContent)
        {
            LastSelectTag = new TagHelperOutput(
                "select",
                new TagHelperAttributeList { { "id", "TestSelect" } },
                (_, _) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()))
            {
                TagMode = TagMode.StartTagAndEndTag
            };

            return Task.FromResult(LastSelectTag);
        }

        protected override Task<string> GetLabelAsHtmlAsync(TagHelperContext context, TagHelperOutput output, TagHelperOutput selectTag)
        {
            return Task.FromResult(string.Empty);
        }

        protected override Task<string> GetValidationAsHtmlAsync(TagHelperContext context, TagHelperOutput output, TagHelperOutput selectTag)
        {
            return Task.FromResult(string.Empty);
        }

        protected override void AddGroupToFormGroupContents(TagHelperContext context, string propertyName, string html, int order, out bool suppress)
        {
            LastGroupHtml = html;
            suppress = false;
        }
    }

    private sealed class FakeTagHelperLocalizer : IAbpTagHelperLocalizer
    {
        public string GetLocalizedText(string text, ModelExplorer explorer) => text;

        public IStringLocalizer? GetLocalizerOrNull(ModelExplorer explorer) => null;

        public IStringLocalizer? GetLocalizerOrNull(Assembly assembly) => null;
    }
}
