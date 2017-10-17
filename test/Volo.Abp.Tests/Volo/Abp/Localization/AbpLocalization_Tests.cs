using System.Globalization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Shouldly;
using Volo.Abp.Localization.Source;
using Volo.Abp.Modularity;
using Volo.Abp.TestBase;
using Xunit;

namespace Volo.Abp.Localization
{
    public class AbpLocalization_Tests : AbpIntegratedTest<AbpLocalization_Tests.TestModule>
    {
        private readonly IStringLocalizer<LocalizationTestResource> _localizer;

        public AbpLocalization_Tests()
        {
            _localizer = GetRequiredService<IStringLocalizer<LocalizationTestResource>>();
        }

        [Fact]
        public void Should_Get_Same_Text_If_Not_Defined_Anywhere()
        {
            const string text = "A string that is not defined anywhere!";

            _localizer[text].Value.ShouldBe(text);
        }

        [Fact]
        public void Should_Get_Localized_Text_If_Defined_In_Current_Culture()
        {
            using (AbpCultureHelper.Use("en"))
            {
                _localizer["Car"].Value.ShouldBe("Car");
                _localizer["CarPlural"].Value.ShouldBe("Cars");
            }

            using (AbpCultureHelper.Use("tr"))
            {
                _localizer["Car"].Value.ShouldBe("Araba");
                _localizer["CarPlural"].Value.ShouldBe("Araba");
            }
        }

        [Fact]
        public void Should_Get_Localized_Text_If_Defined_In_Requested_Culture()
        {
            _localizer.WithCulture(CultureInfo.GetCultureInfo("en"))["Car"].Value.ShouldBe("Car");
            _localizer.WithCulture(CultureInfo.GetCultureInfo("en"))["CarPlural"].Value.ShouldBe("Cars");

            _localizer.WithCulture(CultureInfo.GetCultureInfo("tr"))["CarPlural"].Value.ShouldBe("Araba");
            _localizer.WithCulture(CultureInfo.GetCultureInfo("tr"))["CarPlural"].Value.ShouldBe("Araba");
        }

        [DependsOn(typeof(AbpTestBaseModule))]
        public class TestModule : AbpModule
        {
            public override void ConfigureServices(IServiceCollection services)
            {
                services.Configure<AbpLocalizationOptions>(options =>
                {
                    options.Resources.AddJson<LocalizationTestResource>("en");
                });
            }
        }
    }
}