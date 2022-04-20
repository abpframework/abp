using System.Globalization;
using System.Linq;
using Microsoft.Extensions.Localization;
using Shouldly;
using Volo.Abp.Localization.TestResources.Base.CountryNames;
using Volo.Abp.Localization.TestResources.Base.Validation;
using Volo.Abp.Localization.TestResources.Source;
using Volo.Abp.Modularity;
using Volo.Abp.Testing;
using Volo.Abp.VirtualFileSystem;
using Xunit;

namespace Volo.Abp.Localization;

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
        using (CultureHelper.Use("en"))
        {
            _localizer["Car"].Value.ShouldBe("Car");
            _localizer["CarPlural"].Value.ShouldBe("Cars");
        }

        using (CultureHelper.Use("tr"))
        {
            _localizer["Car"].Value.ShouldBe("Araba");
            _localizer["CarPlural"].Value.ShouldBe("Araba");
        }

        using (CultureHelper.Use("it"))
        {
            _localizer["Car"].Value.ShouldBe("Auto");
        }

        using (CultureHelper.Use("es"))
        {
            _localizer["Car"].Value.ShouldBe("Auto");
        }

        using (CultureHelper.Use("de"))
        {
            _localizer["Car"].Value.ShouldBe("Auto");
        }

    }

    [Fact]
    public void Should_Get_Extension_Texts()
    {
        using (CultureHelper.Use("en"))
        {
            _localizer["SeeYou"].Value.ShouldBe("See you");
        }

        using (CultureHelper.Use("tr"))
        {
            _localizer["SeeYou"].Value.ShouldBe("See you"); //Not defined in tr, getting from default lang
        }

        using (CultureHelper.Use("it"))
        {
            _localizer["SeeYou"].Value.ShouldBe("Ci vediamo");
        }

        using (CultureHelper.Use("es"))
        {
            _localizer["SeeYou"].Value.ShouldBe("Nos vemos");
        }

        using (CultureHelper.Use("de"))
        {
            _localizer["SeeYou"].Value.ShouldBe("Bis bald");
        }

    }

    [Fact]
    public void Should_Get_From_Inherited_Texts()
    {
        using (CultureHelper.Use("en"))
        {
            _localizer["USA"].Value.ShouldBe("United States of America"); //Inherited from CountryNames/en.json
            _localizer["ThisFieldIsRequired"].Value.ShouldBe("This field is required"); //Inherited from Validation/en.json

            _localizer.GetAllStrings().ShouldContain(ls => ls.Name == "USA");
        }

        using (CultureHelper.Use("tr"))
        {
            _localizer["USA"].Value.ShouldBe("Amerika Birleşik Devletleri"); //Inherited from CountryNames/tr.json
        }

        using (CultureHelper.Use("es"))
        {
            _localizer["USA"].Value.ShouldBe("Estados unidos de América"); //Inherited from CountryNames/es.json
            _localizer["ThisFieldIsRequired"].Value.ShouldBe("El campo no puede estar vacío"); //Inherited from Validation/es.json

            _localizer.GetAllStrings().ShouldContain(ls => ls.Name == "USA");
        }

    }

    [Fact]
    public void Should_Override_Inherited_Text()
    {
        using (CultureHelper.Use("en"))
        {
            _localizer["MaxLenghtErrorMessage", 42].Value.ShouldBe("This field's length can be maximum of '42' chars"); //Overriden in Source/en.json
        }

        using (CultureHelper.Use("es"))
        {
            _localizer["MaxLenghtErrorMessage", 42].Value.ShouldBe("El campo puede tener un máximo de '42' caracteres"); //Overriden in Source/es.json
        }

        using (CultureHelper.Use("de"))
        {
            _localizer["MaxLenghtErrorMessage", 42].Value.ShouldBe("Die Länge dieses Feldes kann maximal '42'-Zeichen betragen"); //Overriden in Source/es.json
        }
    }

    [Fact]
    public void Should_Get_Localized_Text_If_Defined_In_Requested_Culture()
    {
        using (CultureHelper.Use(CultureInfo.GetCultureInfo("en")))
        {
            _localizer["Car"].Value.ShouldBe("Car");
        }
        using (CultureHelper.Use(CultureInfo.GetCultureInfo("en")))
        {
            _localizer["CarPlural"].Value.ShouldBe("Cars");
        }

        using (CultureHelper.Use(CultureInfo.GetCultureInfo("tr")))
        {
            _localizer["Car"].Value.ShouldBe("Araba");
        }
        using (CultureHelper.Use(CultureInfo.GetCultureInfo("tr")))
        {
            _localizer["CarPlural"].Value.ShouldBe("Araba");
        }

        using (CultureHelper.Use(CultureInfo.GetCultureInfo("es")))
        {
            _localizer["Car"].Value.ShouldBe("Auto");
        }
        using (CultureHelper.Use(CultureInfo.GetCultureInfo("es")))
        {
            _localizer["CarPlural"].Value.ShouldBe("Autos");
        }

        using (CultureHelper.Use(CultureInfo.GetCultureInfo("zh-Hans")))
        {
            _localizer["Car"].Value.ShouldBe("汽车");
        }
        using (CultureHelper.Use(CultureInfo.GetCultureInfo("zh-Hans")))
        {
            _localizer["CarPlural"].Value.ShouldBe("汽车");
        }

        using (CultureHelper.Use(CultureInfo.GetCultureInfo("zh-CN")))
        {
            _localizer["Car"].Value.ShouldBe("汽车");
        }
        using (CultureHelper.Use(CultureInfo.GetCultureInfo("zh-CN")))
        {
            _localizer["CarPlural"].Value.ShouldBe("汽车");
        }
        
        using (CultureHelper.Use(CultureInfo.GetCultureInfo("zh-Hans-CN")))
        {
            _localizer["Car"].Value.ShouldBe("汽车");
        }
        using (CultureHelper.Use(CultureInfo.GetCultureInfo("zh-Hans-CN")))
        {
            _localizer["CarPlural"].Value.ShouldBe("汽车");
        }

        using (CultureHelper.Use(CultureInfo.GetCultureInfo("zh-Hant")))
        {
            _localizer["Car"].Value.ShouldBe("汽車");
        }
        using (CultureHelper.Use(CultureInfo.GetCultureInfo("zh-Hant")))
        {
            _localizer["CarPlural"].Value.ShouldBe("汽車");
        }
        
        using (CultureHelper.Use(CultureInfo.GetCultureInfo("zh-TW")))
        {
            _localizer["Car"].Value.ShouldBe("汽車");
        }
        using (CultureHelper.Use(CultureInfo.GetCultureInfo("zh-TW")))
        {
            _localizer["CarPlural"].Value.ShouldBe("汽車");
        }
        
        using (CultureHelper.Use(CultureInfo.GetCultureInfo("zh-Hant-TW")))
        {
            _localizer["Car"].Value.ShouldBe("汽車");
        }
        using (CultureHelper.Use(CultureInfo.GetCultureInfo("zh-Hant-TW")))
        {
            _localizer["CarPlural"].Value.ShouldBe("汽車");
        }
    }

    [Fact]
    public void GetAllStrings_With_Parents()
    {
        using (CultureHelper.Use("tr"))
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

        using (CultureHelper.Use("es"))
        {
            var localizedStrings = _localizer.GetAllStrings(true).ToList();

            localizedStrings.ShouldContain(
                ls => ls.Name == "FortyTwo" &&
                      ls.Value == "Curenta y dos" &&
                      ls.ResourceNotFound == false
            );

            localizedStrings.ShouldContain(
                ls => ls.Name == "Universe" &&
                      ls.Value == "Universo" &&
                      ls.ResourceNotFound == false
            );
        }

    }

    [Fact]
    public void GetAllStrings_Without_Parents()
    {
        using (CultureHelper.Use("tr"))
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

        using (CultureHelper.Use("es"))
        {
            var localizedStrings = _localizer.GetAllStrings(false).ToList();

            localizedStrings.ShouldNotContain(
                ls => ls.Name == "FortyThree"
            );

            localizedStrings.ShouldContain(
                ls => ls.Name == "Universe" &&
                      ls.Value == "Universo" &&
                      ls.ResourceNotFound == false
            );
        }

    }

    [Fact]
    public void GetAllStrings_With_Inheritance()
    {
        using (CultureHelper.Use("tr"))
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
        using (CultureHelper.Use("tr"))
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
