using Microsoft.Extensions.Options;
using Shouldly;
using Xunit;

namespace Volo.Abp.TextTemplating
{
    public class AbpTextTemplatingOptions_Tests : AbpTextTemplatingTestBase<AbpTextTemplatingTestModule>
    {
        private readonly AbpTextTemplatingOptions _options;

        public AbpTextTemplatingOptions_Tests()
        {
            _options = GetRequiredService<IOptions<AbpTextTemplatingOptions>>().Value;
        }

        [Fact]
        public void Should_Auto_Add_TemplateDefinitionProviders_To_Options()
        {
            _options
                .DefinitionProviders
                .ShouldContain(typeof(TestTemplateDefinitionProvider));
        }
    }
}
