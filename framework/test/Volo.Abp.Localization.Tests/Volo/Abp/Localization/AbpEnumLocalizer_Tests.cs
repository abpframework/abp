using Microsoft.Extensions.Localization;
using Shouldly;
using Volo.Abp.Localization.TestResources.Base.Validation;
using Volo.Abp.Testing;
using Xunit;

namespace Volo.Abp.Localization;

public class AbpEnumLocalizer_Tests : AbpIntegratedTest<AbpLocalizationTestModule>
{
    private readonly IAbpEnumLocalizer _enumLocalizer;

    public AbpEnumLocalizer_Tests()
    {
        _enumLocalizer = GetRequiredService<IAbpEnumLocalizer>();
    }

    [Fact]
    public void GetString_Test()
    {
        using (CultureHelper.Use("en"))
        {
            _enumLocalizer.GetString<BookType>(BookType.Undefined).ShouldBe("Undefined");
            _enumLocalizer.GetString<BookType>(BookType.Adventure).ShouldBe("Adventure");
            _enumLocalizer.GetString<BookType>(0).ShouldBe("Undefined with value 0");
            _enumLocalizer.GetString<BookType>(1).ShouldBe("Adventure with value 1");
            _enumLocalizer.GetString<BookType>(BookType.Biography).ShouldBe("Biography");

            var specifyLocalizer = new[]
            {
                GetRequiredService<IStringLocalizerFactory>().Create<LocalizationTestValidationResource>()
            };
            _enumLocalizer.GetString<BookType>(BookType.Undefined, specifyLocalizer).ShouldBe("Undefined from ValidationResource");
            _enumLocalizer.GetString<BookType>(BookType.Adventure, specifyLocalizer).ShouldBe("Adventure from ValidationResource");
            _enumLocalizer.GetString<BookType>(0, specifyLocalizer).ShouldBe("Undefined with value 0 from ValidationResource");
            _enumLocalizer.GetString<BookType>(1, specifyLocalizer).ShouldBe("Adventure with value 1 from ValidationResource");
            _enumLocalizer.GetString<BookType>(BookType.Biography, specifyLocalizer).ShouldBe("Biography from ValidationResource");
        }
    }

    [Fact]
    public void GetString_With_Undefined_Value_Test()
    {
        using (CultureHelper.Use("en"))
        {
            _enumLocalizer.GetString<BookType>(42).ShouldBe("42");
            _enumLocalizer.GetString<BookType>((BookType)42).ShouldBe("42");
            _enumLocalizer.GetString<BookPermissions>(BookPermissions.Read | BookPermissions.Write).ShouldBe("Read, Write");
            _enumLocalizer.GetString<BookPermissions>(3).ShouldBe("Read, Write");

            var specifyLocalizer = new[]
            {
                GetRequiredService<IStringLocalizerFactory>().Create<LocalizationTestValidationResource>()
            };
            _enumLocalizer.GetString<BookType>(42, specifyLocalizer).ShouldBe("42");
        }
    }
}

enum BookType
{
    Undefined,
    Adventure,
    Biography,
}

[System.Flags]
enum BookPermissions
{
    None = 0,
    Read = 1,
    Write = 2,
}
