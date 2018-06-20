using Shouldly;
using Xunit;

namespace Volo.Abp.AspNetCore.Mvc.UI.Minification.NUglify
{
    public class NUglifyJavascriptMinifier_Tests : AbpAspNetCoreMvcUiTestBase
    {
        private readonly NUglifyJavascriptMinifier _nUglifyJavascriptMinifier;

        public NUglifyJavascriptMinifier_Tests()
        {
            _nUglifyJavascriptMinifier = GetRequiredService<NUglifyJavascriptMinifier>();
        }

        [Fact]
        public void Should_Minify_Simple_Code()
        {
            const string source = "var x = 5; var y = 6;";

            var minified = _nUglifyJavascriptMinifier.Minify(source);

            minified.Length.ShouldBeGreaterThan(0);
            minified.Length.ShouldBeLessThan(source.Length);
        }
    }
}
