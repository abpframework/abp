#nullable enable

using System.Collections.Generic;
using System.Linq;
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
        service.LastGroupHtml.ShouldContain("aria-describedby=\"TestSelectInfoText\"");
    }

    [Fact]
    public async Task Info_text_should_skip_id_and_aria_describedby_when_select_has_no_id()
    {
        var service = new TestAbpSelectTagHelperService(selectId: null);
        var tagHelper = new AbpSelectTagHelper(service)
        {
            AspFor = CreateModelExpression(),
            InfoText = "Description"
        };

        var output = CreateOutput();

        await tagHelper.ProcessAsync(CreateContext(), output);

        service.LastGroupHtml.ShouldContain("<div class=\"form-text\"");
        service.LastGroupHtml.ShouldContain("Description");
        service.LastGroupHtml.ShouldNotContain("id=\"InfoText\"");
        service.LastGroupHtml.ShouldNotContain("aria-describedby=\"InfoText\"");

        service.LastSelectTag.ShouldNotBeNull();
        service.LastSelectTag!.Attributes.ContainsName("aria-describedby").ShouldBeFalse();
    }

    [Fact]
    public async Task Aria_describedby_should_preserve_existing_value_set_by_caller()
    {
        var service = new TestAbpSelectTagHelperService(existingAriaDescribedby: "custom-id");
        var tagHelper = new AbpSelectTagHelper(service)
        {
            AspFor = CreateModelExpression(),
            InfoText = "Description"
        };

        var output = CreateOutput();

        await tagHelper.ProcessAsync(CreateContext(), output);

        service.LastSelectTag.ShouldNotBeNull();
        service.LastSelectTag!.Attributes["aria-describedby"].Value.ToString().ShouldBe("custom-id TestSelectInfoText");
        service.LastGroupHtml.ShouldContain("aria-describedby=\"custom-id TestSelectInfoText\"");
    }

    [Fact]
    public async Task Aria_describedby_should_split_on_html_whitespace_separators()
    {
        var service = new TestAbpSelectTagHelperService(existingAriaDescribedby: "id1\tid2");
        var tagHelper = new AbpSelectTagHelper(service)
        {
            AspFor = CreateModelExpression(),
            InfoText = "Description"
        };

        var output = CreateOutput();

        await tagHelper.ProcessAsync(CreateContext(), output);

        service.LastSelectTag.ShouldNotBeNull();
        service.LastSelectTag!.Attributes["aria-describedby"].Value.ToString().ShouldBe("id1\tid2 TestSelectInfoText");
    }

    [Fact]
    public async Task InputInfoText_attribute_should_render_info_text_with_single_aria_describedby()
    {
        var service = new TestAbpSelectTagHelperService();
        var tagHelper = new AbpSelectTagHelper(service)
        {
            AspFor = CreateModelExpressionWithInputInfoText()
        };

        var output = CreateOutput();

        await tagHelper.ProcessAsync(CreateContext(), output);

        service.LastGroupHtml.ShouldContain("<div class=\"form-text\"");
        service.LastGroupHtml.ShouldContain("Description from attribute");
        service.LastGroupHtml.ShouldNotContain("<small");

        service.LastSelectTag.ShouldNotBeNull();
        var ariaDescribedby = service.LastSelectTag!.Attributes.Where(a => a.Name == "aria-describedby").ToList();
        ariaDescribedby.Count.ShouldBe(1);
        ariaDescribedby[0].Value.ToString().ShouldBe("TestSelectInfoText");
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

    private static ModelExpression CreateModelExpressionWithInputInfoText()
    {
        var metadataProvider = new EmptyModelMetadataProvider();
        var modelExplorer = metadataProvider
            .GetModelExplorerForType(typeof(TestModelWithInputInfoText), null)
            .GetExplorerForProperty(nameof(TestModelWithInputInfoText.TestSelect));
        return new ModelExpression(nameof(TestModelWithInputInfoText.TestSelect), modelExplorer);
    }

    private class TestModelWithInputInfoText
    {
        [InputInfoText("Description from attribute")]
        public string TestSelect { get; set; } = string.Empty;
    }

    private sealed class TestAbpSelectTagHelperService : AbpSelectTagHelperService
    {
        private readonly string? _selectId;
        private readonly string? _existingAriaDescribedby;

        public string LastGroupHtml { get; private set; } = string.Empty;

        public TagHelperOutput? LastSelectTag { get; private set; }

        public TestAbpSelectTagHelperService(string? selectId = "TestSelect", string? existingAriaDescribedby = null)
            : base(null!, HtmlEncoder.Default, new FakeTagHelperLocalizer(), null!, null!)
        {
            _selectId = selectId;
            _existingAriaDescribedby = existingAriaDescribedby;
        }

        protected override Task<TagHelperOutput> GetSelectTagAsync(TagHelperContext context, TagHelperOutput output, TagHelperContent childContent)
        {
            var attributes = new TagHelperAttributeList();
            if (!string.IsNullOrEmpty(_selectId))
            {
                attributes.Add("id", _selectId);
            }
            if (!string.IsNullOrEmpty(_existingAriaDescribedby))
            {
                attributes.Add("aria-describedby", _existingAriaDescribedby);
            }

            LastSelectTag = new TagHelperOutput(
                "select",
                attributes,
                (_, _) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()))
            {
                TagMode = TagMode.StartTagAndEndTag
            };

            AddInfoTextId(LastSelectTag);

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
