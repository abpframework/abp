using Shouldly;
using Volo.Abp.Minify.Styles;
using Volo.Abp.Testing;
using Xunit;

namespace Volo.Abp.Minify.NUglify;

public class CssMinifier_Tests : AbpIntegratedTest<AbpMinifyModule>
{
    private readonly ICssMinifier _cssMinifier;

    public CssMinifier_Tests()
    {
        _cssMinifier = GetRequiredService<ICssMinifier>();
    }

    [Fact]
    public void Should_Minify_Simple_Code()
    {
        const string source = "div { color: #FFF; }";

        var minified = _cssMinifier.Minify(source);

        minified.Length.ShouldBeGreaterThan(0);
        minified.Length.ShouldBeLessThan(source.Length);
    }
}
