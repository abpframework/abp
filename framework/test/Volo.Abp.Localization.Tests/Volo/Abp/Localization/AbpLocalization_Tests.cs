using System.Globalization;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Shouldly;
using Volo.Abp.Localization.TestResources.Base.CountryNames;
using Volo.Abp.Localization.TestResources.Base.Validation;
using Volo.Abp.Localization.TestResources.Source;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;
using Xunit;

namespace Volo.Abp.Localization
{
    public class AbpLocalization_Tests : AbpIntegratedTest<AbpLocalization_Tests.TestModule>
    {
        private readonly IStringLocalizer<LocalizationTestResource> _localizer;
        private readonly IStringLocalizerFactory _localizerFactory;

        public AbpLocalization_Tests()
        {
            _localizer = GetRequiredService<IStringLocalizer<LocalizationTestResource>>();
            _localizerFactory = GetRequiredService<IStringLocalizerFactory>();
        }

        [Fact]
        public void AbpStringLocalizerExtensions_GetInternalLocalizer()
        {
            var internalLocalizer = _localizer.GetInternalLocalizer();
            internalLocalizer.ShouldNotBeNull();
            internalLocalizer.ShouldBeOfType<AbpDictionaryBasedStringLocalizer>();
        }

        [Fact]
        public void AbpStringLocalizerExtensions_GetInternalLocalizer_Using_LocalizerFactory()
        {
            var internalLocalizer = _localizerFactory.Create(typeof(LocalizationTestResource)).GetInternalLocalizer();
            internalLocalizer.ShouldNotBeNull();
            internalLocalizer.ShouldBeOfType<AbpDictionaryBasedStringLocalizer>();
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

                _localizer.GetAllStrings().ShouldContain(ls => ls.Name == "USA");
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

        [Fact]
        public void GetAllStrings_With_Parents()
        {
            using (AbpCultureHelper.Use("tr"))
            {
                var localizedStrings = _localizer.GetAllStrings(true).ToList();

                localizedStrings.ShouldContain(
                    ls => ls.Name == "FortyTwo" &&
                          ls.Value == "Forty Two" &&
                          ls.ResourceNotFound == false
                );

                localizedStrings.ShouldContain(
                    ls => ls.Name == "Universe" &&
                          ls.Value == "Evren" &&
                          ls.ResourceNotFound == false
                );
            }
        }

        [Fact]
        public void GetAllStrings_Without_Parents()
        {
            using (AbpCultureHelper.Use("tr"))
            {
                var localizedStrings = _localizer.GetAllStrings(false).ToList();

                localizedStrings.ShouldNotContain(
                    ls => ls.Name == "FortyTwo"
                );

                localizedStrings.ShouldContain(
                    ls => ls.Name == "Universe" &&
                          ls.Value == "Evren" &&
                          ls.ResourceNotFound == false
                );
            }
        }

        [Fact]
        public void GetAllStrings_With_Inheritance()
        {
            using (AbpCultureHelper.Use("tr"))
            {
                var localizedStrings = _localizer
                    .GetAllStrings(true, includeBaseLocalizers: true)
                    .ToList();

                localizedStrings.ShouldContain(
                    ls => ls.Name == "USA" &&
                          ls.Value == "Amerika Birleşik Devletleri" &&
                          ls.ResourceNotFound == false
                );

                localizedStrings.ShouldContain(
                    ls => ls.Name == "Universe" &&
                          ls.Value == "Evren" &&
                          ls.ResourceNotFound == false
                );

                localizedStrings.ShouldContain(
                    ls => ls.Name == "SeeYou" &&
                          ls.Value == "See you" &&
                          ls.ResourceNotFound == false
                );
            }
        }

        [Fact]
        public void GetAllStrings_Without_Inheritance()
        {
            using (AbpCultureHelper.Use("tr"))
            {
                var localizedStrings = _localizer
                    .GetAllStrings(true, includeBaseLocalizers: false)
                    .ToList();

                localizedStrings.ShouldNotContain(
                    ls => ls.Name == "USA"
                );

                localizedStrings.ShouldContain(
                    ls => ls.Name == "Universe" &&
                          ls.Value == "Evren" &&
                          ls.ResourceNotFound == false
                );

                localizedStrings.ShouldContain(
                    ls => ls.Name == "SeeYou" &&
                          ls.Value == "See you" &&
                          ls.ResourceNotFound == false
                );
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
                    options.FileSets.AddEmbedded<TestModule>();
                });

                Configure<AbpLocalizationOptions>(options =>
                {
                    options.Resources
                        .Add<LocalizationTestValidationResource>("en")
                        .AddVirtualJson("/Volo/Abp/Localization/TestResources/Base/Validation");

                    options.Resources
                        .Add<LocalizationTestCountryNamesResource>("en")
                        .AddVirtualJson("/Volo/Abp/Localization/TestResources/Base/CountryNames");

                    options.Resources
                        .Add<LocalizationTestResource>("en")
                        .AddVirtualJson("/Volo/Abp/Localization/TestResources/Source");

                    options.Resources
                        .Get<LocalizationTestResource>()
                        .AddVirtualJson("/Volo/Abp/Localization/TestResources/SourceExt");
                });
            }
        }
    }
}