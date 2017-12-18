using System.Globalization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Shouldly;
using Volo.Abp.Localization.Base.CountryNames;
using Volo.Abp.Localization.Base.Validation;
using Volo.Abp.Localization.Source;
using Volo.Abp.Localization.SourceExt;
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

            using (AbpCultureHelper.Use("it"))
            {
                _localizer["Car"].Value.ShouldBe("Auto");
            }
        }

        [Fact]
        public void Should_Get_Extension_Texts()
        {
            using (AbpCultureHelper.Use("en"))
            {
                _localizer["SeeYou"].Value.ShouldBe("See you");
            }

            using (AbpCultureHelper.Use("tr"))
            {
                _localizer["SeeYou"].Value.ShouldBe("See you"); //Not defined in tr, getting from default lang
            }

            using (AbpCultureHelper.Use("it"))
            {
                _localizer["SeeYou"].Value.ShouldBe("Ci vediamo");
            }
        }

        [Fact]
        public void Should_Get_From_Inherited_Texts()
        {
            using (AbpCultureHelper.Use("en"))
            {
                _localizer["USA"].Value.ShouldBe("United States of America"); //Inherited from CountryNames/en.json
                _localizer["ThisFieldIsRequired"].Value.ShouldBe("This field is required"); //Inherited from Validation/en.json
            }

            using (AbpCultureHelper.Use("tr"))
            {
                _localizer["USA"].Value.ShouldBe("Amerika Birleşik Devletleri"); //Inherited from CountryNames/tr.json
            }
        }

        [Fact]
        public void Should_Override_Inherited_Text()
        {
            using (AbpCultureHelper.Use("en"))
            {
                _localizer["MaxLenghtErrorMessage", 42].Value.ShouldBe("This field's length can be maximum of '42' chars"); //Overriden in Source/en.json
            }
        }

        [Fact]
        public void Should_Get_Localized_Text_If_Defined_In_Requested_Culture()
        {
            _localizer.WithCulture(CultureInfo.GetCultureInfo("en"))["Car"].Value.ShouldBe("Car");
            _localizer.WithCulture(CultureInfo.GetCultureInfo("en"))["CarPlural"].Value.ShouldBe("Cars");

            _localizer.WithCulture(CultureInfo.GetCultureInfo("tr"))["Car"].Value.ShouldBe("Araba");
            _localizer.WithCulture(CultureInfo.GetCultureInfo("tr"))["CarPlural"].Value.ShouldBe("Araba");
        }

        [DependsOn(typeof(AbpTestBaseModule))]
        [DependsOn(typeof(AbpLocalizationModule))]
        public class TestModule : AbpModule
        {
            public override void ConfigureServices(IServiceCollection services)
            {
                services.Configure<AbpLocalizationOptions>(options =>
                {
                    options.Resources.AddJson<LocalizationTestValidationResource>("en");
                    options.Resources.AddJson<LocalizationTestCountryNamesResource>("en");
                    options.Resources.AddJson<LocalizationTestResource>("en");
                    options.Resources.ExtendWithJson<LocalizationTestResource, LocalizationTestResourceExt>();
                });
            }
        }
    }
}