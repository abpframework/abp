using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Volo.Abp.AspNetCore.Components.Web.Theming.Bundling;

public class ComponentBundleUrlBuilder_Tests
{
    private readonly IComponentBundleUrlBuilder _builder = new ComponentBundleUrlBuilder();

    [Fact]
    public async Task Should_Return_FileName_When_No_PathBase_Available()
    {
        (await _builder.BuildAsync("/__bundles/Global.css", appBasePath: null, navigationBaseUri: null))
            .ShouldBe("/__bundles/Global.css");
    }

    [Fact]
    public async Task Should_Return_FileName_When_NavigationBaseUri_Has_Root_PathBase()
    {
        (await _builder.BuildAsync("/__bundles/Global.css", appBasePath: null, navigationBaseUri: "https://localhost/"))
            .ShouldBe("/__bundles/Global.css");
    }

    [Fact]
    public async Task Should_Use_Explicit_AppBasePath_Over_NavigationBaseUri()
    {
        (await _builder.BuildAsync("/__bundles/Global.css", appBasePath: "/explicit", navigationBaseUri: "https://localhost/from-nav/"))
            .ShouldBe("/explicit/__bundles/Global.css");
    }

    [Fact]
    public async Task Should_Resolve_PathBase_From_NavigationBaseUri()
    {
        (await _builder.BuildAsync("/__bundles/Global.css", appBasePath: null, navigationBaseUri: "https://localhost/foo/"))
            .ShouldBe("/foo/__bundles/Global.css");
    }

    [Fact]
    public async Task Should_Resolve_PathBase_From_NavigationBaseUri_When_AppBasePath_Empty()
    {
        (await _builder.BuildAsync("/__bundles/Global.css", appBasePath: "", navigationBaseUri: "https://localhost/foo/"))
            .ShouldBe("/foo/__bundles/Global.css");
    }

    [Fact]
    public async Task Should_Handle_Nested_PathBase()
    {
        (await _builder.BuildAsync("/__bundles/Global.css", appBasePath: null, navigationBaseUri: "https://localhost/foo/bar/"))
            .ShouldBe("/foo/bar/__bundles/Global.css");
    }

    [Fact]
    public async Task Should_Normalize_AppBasePath_Without_Trailing_Slash()
    {
        (await _builder.BuildAsync("/__bundles/Global.css", appBasePath: "/foo", navigationBaseUri: null))
            .ShouldBe("/foo/__bundles/Global.css");
    }

    [Fact]
    public async Task Should_Not_Duplicate_Leading_Slash()
    {
        (await _builder.BuildAsync("/__bundles/Global.css", appBasePath: "/foo/", navigationBaseUri: null))
            .ShouldBe("/foo/__bundles/Global.css");
    }

    [Fact]
    public async Task Should_Handle_FileName_Without_Leading_Slash()
    {
        (await _builder.BuildAsync("__bundles/Global.css", appBasePath: "/foo", navigationBaseUri: null))
            .ShouldBe("/foo/__bundles/Global.css");
    }

    [Fact]
    public async Task Should_Return_FileName_When_NavigationBaseUri_Is_Invalid()
    {
        (await _builder.BuildAsync("/__bundles/Global.css", appBasePath: null, navigationBaseUri: "not-a-uri"))
            .ShouldBe("/__bundles/Global.css");
    }

    [Fact]
    public async Task Should_Throw_When_FileName_Is_Null()
    {
        await Should.ThrowAsync<System.ArgumentNullException>(async () =>
            await _builder.BuildAsync(null!, appBasePath: "/foo", navigationBaseUri: null));
    }

    [Theory]
    [InlineData("https://cdn.example.com/foo.css")]
    [InlineData("http://cdn.example.com/foo.css")]
    [InlineData("//cdn.example.com/foo.css")]
    [InlineData("data:text/css;base64,Zm9vIA==")]
    public async Task Should_Not_Prefix_External_Urls(string externalUrl)
    {
        (await _builder.BuildAsync(externalUrl, appBasePath: "/foo", navigationBaseUri: "https://localhost/foo/"))
            .ShouldBe(externalUrl);
    }

    [Fact]
    public async Task Should_Treat_Whitespace_AppBasePath_As_Not_Provided()
    {
        (await _builder.BuildAsync("/__bundles/Global.css", appBasePath: "   ", navigationBaseUri: "https://localhost/foo/"))
            .ShouldBe("/foo/__bundles/Global.css");
    }

    [Fact]
    public async Task Should_Be_Idempotent_For_Already_Prefixed_FileName()
    {
        (await _builder.BuildAsync("/foo/__bundles/Global.css", appBasePath: "/foo", navigationBaseUri: null))
            .ShouldBe("/foo/__bundles/Global.css");
    }

    [Fact]
    public async Task Should_Be_Idempotent_When_PathBase_Resolved_From_NavigationBaseUri()
    {
        (await _builder.BuildAsync("/foo/__bundles/Global.css", appBasePath: null, navigationBaseUri: "https://localhost/foo/"))
            .ShouldBe("/foo/__bundles/Global.css");
    }
}
