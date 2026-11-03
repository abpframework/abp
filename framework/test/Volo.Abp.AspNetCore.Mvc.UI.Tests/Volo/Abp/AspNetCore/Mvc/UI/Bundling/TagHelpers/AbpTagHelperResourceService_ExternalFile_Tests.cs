#nullable enable

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Shouldly;
using Volo.Abp.AspNetCore.Security;
using Xunit;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling.TagHelpers;

public class AbpTagHelperResourceService_ExternalFile_Tests
{
    [Theory]
    [InlineData("/themes/basic/googlefonts.css", "", "/themes/basic/googlefonts.css")]
    [InlineData("/themes/basic/googlefonts.css", "/foo", "/foo/themes/basic/googlefonts.css")]
    [InlineData("~/themes/basic/googlefonts.css", "/foo", "/foo/themes/basic/googlefonts.css")]
    [InlineData("/themes/basic/googlefonts.css", "/foo/bar", "/foo/bar/themes/basic/googlefonts.css")]
    [InlineData("https://fonts.googleapis.com/css2?family=Lexend", "/foo", "https://fonts.googleapis.com/css2?family=Lexend")]
    [InlineData("//cdn.example.com/foo.css", "/foo", "//cdn.example.com/foo.css")]
    [InlineData("main.css", "/foo", "main.css")]
    [InlineData("/foo/themes/basic/googlefonts.css", "/foo", "/foo/themes/basic/googlefonts.css")]
    [InlineData("/foo/bar/themes/basic/googlefonts.css", "/foo/bar", "/foo/bar/themes/basic/googlefonts.css")]
    [InlineData("/foo/themes/basic/googlefonts.css?v=1", "/foo", "/foo/themes/basic/googlefonts.css?v=1")]
    [InlineData("/foobar/themes/basic/googlefonts.css", "/foo", "/foo/foobar/themes/basic/googlefonts.css")]
    [InlineData("~/themes/basic/googlefonts.css", "", "/themes/basic/googlefonts.css")]
    [InlineData("~/themes/basic/googlefonts.css", "/themes", "/themes/themes/basic/googlefonts.css")]
    [InlineData("~/themes/basic/googlefonts.css", "/themes/basic", "/themes/basic/themes/basic/googlefonts.css")]
    public void Style_Should_Resolve_Local_External_File_With_PathBase(string fileName, string pathBase, string expected)
    {
        var service = new TestStyleService();

        var output = CreateOutput();
        service.AddTag(CreateViewContext(pathBase), output, new BundleFile(fileName, true));

        output.Content.GetContent().ShouldContain($"href=\"{expected}\"");
    }

    [Theory]
    [InlineData("/libs/external.js", "", "/libs/external.js")]
    [InlineData("/libs/external.js", "/foo", "/foo/libs/external.js")]
    [InlineData("~/libs/external.js", "/foo", "/foo/libs/external.js")]
    [InlineData("https://cdn.example.com/external.js", "/foo", "https://cdn.example.com/external.js")]
    [InlineData("//cdn.example.com/external.js", "/foo", "//cdn.example.com/external.js")]
    [InlineData("/foo/libs/external.js", "/foo", "/foo/libs/external.js")]
    [InlineData("/foo/bar/libs/external.js", "/foo/bar", "/foo/bar/libs/external.js")]
    [InlineData("/foobar/libs/external.js", "/foo", "/foo/foobar/libs/external.js")]
    public void Script_Should_Resolve_Local_External_File_With_PathBase(string fileName, string pathBase, string expected)
    {
        var service = new TestScriptService();

        var output = CreateOutput();
        service.AddTag(CreateViewContext(pathBase), output, new BundleFile(fileName, true));

        output.Content.GetContent().ShouldContain($"src=\"{expected}\"");
    }

    private static ViewContext CreateViewContext(string pathBase)
    {
        var httpContext = new DefaultHttpContext
        {
            RequestServices = new ServiceCollection()
                .AddSingleton<IUrlHelperFactory, UrlHelperFactory>()
                .BuildServiceProvider()
        };
        httpContext.Request.PathBase = new PathString(pathBase);

        return new ViewContext
        {
            HttpContext = httpContext,
            RouteData = new RouteData(),
            ActionDescriptor = new ActionDescriptor()
        };
    }

    private static TagHelperOutput CreateOutput()
    {
        return new TagHelperOutput(
            "abp-test",
            new TagHelperAttributeList(),
            (_, _) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));
    }

    private static TagHelperContext CreateContext()
    {
        return new TagHelperContext(new TagHelperAttributeList(), new Dictionary<object, object>(), "test");
    }

    private class TestTagHelper : TagHelper
    {
    }

    private class TestStyleService : AbpTagHelperStyleService
    {
        public TestStyleService()
            : base(null!, Microsoft.Extensions.Options.Options.Create(new AbpBundlingOptions()), null!, Microsoft.Extensions.Options.Options.Create(new AbpSecurityHeadersOptions()))
        {
        }

        public void AddTag(ViewContext viewContext, TagHelperOutput output, BundleFile file)
        {
            AddHtmlTag(viewContext, new TestTagHelper(), CreateContext(), output, file, null);
        }
    }

    private class TestScriptService : AbpTagHelperScriptService
    {
        public TestScriptService()
            : base(null!, Microsoft.Extensions.Options.Options.Create(new AbpBundlingOptions()), null!)
        {
        }

        public void AddTag(ViewContext viewContext, TagHelperOutput output, BundleFile file)
        {
            AddHtmlTag(viewContext, new TestTagHelper(), CreateContext(), output, file, null);
        }
    }
}
