using System.Collections.Generic;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
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
}
