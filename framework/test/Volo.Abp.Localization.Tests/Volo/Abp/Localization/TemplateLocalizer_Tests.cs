using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Shouldly;
using Volo.Abp.Localization.TestResources.Source;
using Volo.Abp.Modularity;
using Volo.Abp.Testing;
using Volo.Abp.VirtualFileSystem;
using Xunit;

namespace Volo.Abp.Localization
{
    public class TemplateLocalizer_Tests : AbpIntegratedTest<TemplateLocalizer_Tests.TestModule>
    {
        private readonly ITemplateLocalizer _templateLocalizer;
        private readonly IStringLocalizer<LocalizationTestResource> _testResource;

        public TemplateLocalizer_Tests()
        {
            _testResource = GetRequiredService<IStringLocalizer<LocalizationTestResource>>();
            _templateLocalizer = GetRequiredService<ITemplateLocalizer>();
        }

        [Fact]
        public void Should_Localize()
        {
            using (CultureHelper.Use("en"))
            {
                _templateLocalizer.Localize(_testResource, "<p>{{#L:CarPlural}} <b>{{#L:Universe}}</b></p>")
                    .ShouldBe("<p>Cars <b>Universe</b></p>");
            }
        }

        [Fact]
        public void Should_Work_Even_If_No_Text_To_Localize()
        {
            using (CultureHelper.Use("en"))
            {
                _templateLocalizer.Localize(_testResource, "<p>test</p>")
                    .ShouldBe("<p>test</p>");
            }
        }

        [DependsOn(typeof(AbpTestBaseModule))]
        [DependsOn(typeof(AbpLocalizationModule))]
        public class TestModule : AbpModule
        {
            public override void ConfigureServices(ServiceConfigurationContext context)
            {
                Configure<AbpVirtualFileSystemOptions>(options =>
                {
                    options.FileSets.AddEmbedded<AbpLocalization_Tests.TestModule>();
                });

                Configure<AbpLocalizationOptions>(options =>
                {
                    options.Resources
                        .Add<LocalizationTestResource>("en")
                        .AddVirtualJson("/Volo/Abp/Localization/TestResources/Source");
                });
            }
        }
    }
}
