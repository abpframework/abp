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

public class AbpInputTagHelperService_Tests
{
    [Fact]
    public async Task Hidden_inputs_should_not_add_margin_bottom_classes()
    {
        var service = new TestAbpInputTagHelperService("hidden");
        var tagHelper = new AbpInputTagHelper(service)
        {
            AspFor = CreateModelExpression()
        };

        var output = CreateOutput();

        await tagHelper.ProcessAsync(CreateContext(), output);

        output.Attributes.ContainsName("class").ShouldBeFalse();
        service.LastGroupHtml.ShouldNotContain("mb-3");
    }

    [Fact]
    public async Task Visible_inputs_should_keep_margin_bottom_classes()
    {
        var service = new TestAbpInputTagHelperService("text");
        var tagHelper = new AbpInputTagHelper(service)
        {
            AspFor = CreateModelExpression()
        };

        var output = CreateOutput();

        await tagHelper.ProcessAsync(CreateContext(), output);

        output.Attributes["class"].Value.ShouldBe("mb-3");
        service.LastGroupHtml.ShouldContain("mb-3");
    }

    [Fact]
    public async Task Info_text_should_be_rendered_as_div_with_form_text_class()
    {
        var service = new TestAbpInputTagHelperServiceForInfo();
        var tagHelper = new AbpInputTagHelper(service)
        {
            AspFor = CreateModelExpression(),
            InfoText = "Description"
        };

        var output = CreateOutput();

        await tagHelper.ProcessAsync(CreateContext(), output);

        service.LastGroupHtml.ShouldContain("<div class=\"form-text\"");
        service.LastGroupHtml.ShouldContain("id=\"TestInputInfoText\"");
        service.LastGroupHtml.ShouldNotContain("<small");
    }

    [Fact]
    public async Task Info_text_should_set_aria_describedby_on_input()
    {
        var service = new TestAbpInputTagHelperServiceForInfo();
        var tagHelper = new AbpInputTagHelper(service)
        {
            AspFor = CreateModelExpression(),
            InfoText = "Description"
        };

        var output = CreateOutput();

        await tagHelper.ProcessAsync(CreateContext(), output);

        service.LastInputTag.ShouldNotBeNull();
        service.LastInputTag!.Attributes["aria-describedby"].Value.ToString().ShouldBe("TestInputInfoText");
        service.LastGroupHtml.ShouldContain("aria-describedby=\"TestInputInfoText\"");
    }

    [Fact]
    public async Task Info_text_should_skip_id_and_aria_describedby_when_input_has_no_id()
    {
        var service = new TestAbpInputTagHelperServiceForInfo(inputId: null);
        var tagHelper = new AbpInputTagHelper(service)
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

        service.LastInputTag.ShouldNotBeNull();
        service.LastInputTag!.Attributes.ContainsName("aria-describedby").ShouldBeFalse();
    }

    [Fact]
    public async Task Aria_describedby_should_preserve_existing_value_set_by_caller()
    {
        var service = new TestAbpInputTagHelperServiceForInfo(existingAriaDescribedby: "custom-id");
        var tagHelper = new AbpInputTagHelper(service)
        {
            AspFor = CreateModelExpression(),
            InfoText = "Description"
        };

        var output = CreateOutput();

        await tagHelper.ProcessAsync(CreateContext(), output);

        service.LastInputTag.ShouldNotBeNull();
        service.LastInputTag!.Attributes["aria-describedby"].Value.ToString().ShouldBe("custom-id TestInputInfoText");
        service.LastGroupHtml.ShouldContain("aria-describedby=\"custom-id TestInputInfoText\"");
    }

    [Fact]
    public async Task InputInfoText_attribute_should_render_info_text_with_single_aria_describedby()
    {
        var service = new TestAbpInputTagHelperServiceForInfo();
        var tagHelper = new AbpInputTagHelper(service)
        {
            AspFor = CreateModelExpressionWithInputInfoText()
        };

        var output = CreateOutput();

        await tagHelper.ProcessAsync(CreateContext(), output);

        service.LastGroupHtml.ShouldContain("<div class=\"form-text\"");
        service.LastGroupHtml.ShouldContain("Description from attribute");
        service.LastGroupHtml.ShouldNotContain("<small");

        service.LastInputTag.ShouldNotBeNull();
        var ariaDescribedby = service.LastInputTag!.Attributes.Where(a => a.Name == "aria-describedby").ToList();
        ariaDescribedby.Count.ShouldBe(1);
        ariaDescribedby[0].Value.ToString().ShouldBe("TestInputInfoText");
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
            "abp-input",
            new TagHelperAttributeList(),
            (_, _) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));
    }

    private static ModelExpression CreateModelExpression()
    {
        var metadataProvider = new EmptyModelMetadataProvider();
        return new ModelExpression(
            "HiddenInput",
            metadataProvider.GetModelExplorerForType(typeof(string), null));
    }

    private static ModelExpression CreateModelExpressionWithInputInfoText()
    {
        var metadataProvider = new EmptyModelMetadataProvider();
        var modelExplorer = metadataProvider
            .GetModelExplorerForType(typeof(TestModelWithInputInfoText), null)
            .GetExplorerForProperty(nameof(TestModelWithInputInfoText.TestInput));
        return new ModelExpression(nameof(TestModelWithInputInfoText.TestInput), modelExplorer);
    }

    private class TestModelWithInputInfoText
    {
        [InputInfoText("Description from attribute")]
        public string TestInput { get; set; } = string.Empty;
    }

    private sealed class TestAbpInputTagHelperService : AbpInputTagHelperService
    {
        private readonly string _inputTypeName;

        public string LastGroupHtml { get; private set; } = string.Empty;

        public TestAbpInputTagHelperService(string inputTypeName)
            : base(null!, HtmlEncoder.Default, null!)
        {
            _inputTypeName = inputTypeName;
        }

        protected override Task<(TagHelperOutput, bool)> GetInputTagHelperOutputAsync(TagHelperContext context, TagHelperOutput output)
        {
            var inputTagHelperOutput = new TagHelperOutput(
                "input",
                new TagHelperAttributeList
                {
                    { "type", _inputTypeName },
                    { "id", "HiddenInput" },
                    { "class", "form-control" }
                },
                (_, _) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

            inputTagHelperOutput.TagMode = TagMode.SelfClosing;

            return Task.FromResult((inputTagHelperOutput, false));
        }

        protected override Task<string> GetLabelAsHtmlAsync(TagHelperContext context, TagHelperOutput output, TagHelperOutput inputTag, bool isCheckbox)
        {
            return Task.FromResult(string.Empty);
        }

        protected override Task<string> GetValidationAsHtmlAsync(TagHelperContext context, TagHelperOutput output, TagHelperOutput inputTag)
        {
            return Task.FromResult(string.Empty);
        }

        protected override string GetInfoAsHtml(TagHelperContext context, TagHelperOutput output, TagHelperOutput inputTag, bool isCheckbox)
        {
            return string.Empty;
        }

        protected override void AddGroupToFormGroupContents(TagHelperContext context, string propertyName, string html, int order, out bool suppress)
        {
            LastGroupHtml = html;
            suppress = false;
        }
    }

    private sealed class TestAbpInputTagHelperServiceForInfo : AbpInputTagHelperService
    {
        private readonly string? _inputId;
        private readonly string? _existingAriaDescribedby;

        public string LastGroupHtml { get; private set; } = string.Empty;

        public TagHelperOutput? LastInputTag { get; private set; }

        public TestAbpInputTagHelperServiceForInfo(string? inputId = "TestInput", string? existingAriaDescribedby = null)
            : base(null!, HtmlEncoder.Default, new FakeTagHelperLocalizer())
        {
            _inputId = inputId;
            _existingAriaDescribedby = existingAriaDescribedby;
        }

        protected override Task<(TagHelperOutput, bool)> GetInputTagHelperOutputAsync(TagHelperContext context, TagHelperOutput output)
        {
            var attributes = new TagHelperAttributeList
            {
                { "type", "text" },
                { "class", "form-control" }
            };
            if (!string.IsNullOrEmpty(_inputId))
            {
                attributes.Add("id", _inputId);
            }
            if (!string.IsNullOrEmpty(_existingAriaDescribedby))
            {
                attributes.Add("aria-describedby", _existingAriaDescribedby);
            }

            LastInputTag = new TagHelperOutput(
                "input",
                attributes,
                (_, _) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()))
            {
                TagMode = TagMode.SelfClosing
            };

            AddInfoTextId(LastInputTag);

            return Task.FromResult((LastInputTag, false));
        }

        protected override Task<string> GetLabelAsHtmlAsync(TagHelperContext context, TagHelperOutput output, TagHelperOutput inputTag, bool isCheckbox)
        {
            return Task.FromResult(string.Empty);
        }

        protected override Task<string> GetValidationAsHtmlAsync(TagHelperContext context, TagHelperOutput output, TagHelperOutput inputTag)
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
