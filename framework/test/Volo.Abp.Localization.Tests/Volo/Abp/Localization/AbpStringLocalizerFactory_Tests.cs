using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using Shouldly;
using Volo.Abp.DynamicProxy;
using Volo.Abp.Localization.TestResources.External;
using Volo.Abp.Testing;
using Xunit;

namespace Volo.Abp.Localization;

public class AbpStringLocalizerFactory_Tests : AbpIntegratedTest<AbpLocalizationTestModule>
{
    private readonly IStringLocalizerFactory _factory;

    public AbpStringLocalizerFactory_Tests()
    {
        _factory = GetRequiredService<IStringLocalizerFactory>();
    }

    [Fact]
    public void Factory_Type_Should_Be_AbpStringLocalizerFactory()
    {
        ProxyHelper.UnProxy(_factory).ShouldBeOfType<AbpStringLocalizerFactory>();
    }
    
    [Fact]
    public void Should_Create_Resource_By_Name()
    {
        using (CultureHelper.Use("en"))
        {
            var localizer = _factory.CreateByResourceNameOrNull("Test");
            localizer.ShouldNotBeNull();
            localizer["CarPlural"].Value.ShouldBe("Cars");
        }
    }
    
    [Fact]
    public async Task Should_Create_Resource_By_Name_Async()
    {
        using (CultureHelper.Use("en"))
        {
            var localizer = await _factory.CreateByResourceNameOrNullAsync("Test");
            localizer.ShouldNotBeNull();
            localizer["CarPlural"].Value.ShouldBe("Cars");
        }
    }

    [Fact]
    public void Should_Throw_Exception_For_Unknown_Resource_Names()
    {
        Assert.Throws<AbpException>(
            () => _factory.CreateByResourceName("UnknownResourceName")
        );
    }

    [Fact]
    public async Task Should_Throw_Exception_For_Unknown_Resource_Names_Async()
    {
        await Assert.ThrowsAsync<AbpException>(
            async () => await _factory.CreateByResourceNameAsync("UnknownResourceName")
        );
    }

    [Fact]
    public void Should_Create_External_Resource_By_Name()
    {
        using (CultureHelper.Use("en"))
        {
            var localizer = _factory.CreateByResourceNameOrNull(TestExternalLocalizationStore.TestExternalResourceNames.ExternalResource1);
            localizer.ShouldNotBeNull();
            localizer["CarPlural"].Value.ShouldBe("CarPlural");
            
            var localizer2 = _factory.CreateByResourceNameOrNull(TestExternalLocalizationStore.TestExternalResourceNames.ExternalResource2);
            localizer2.ShouldNotBeNull();
            localizer2["CarPlural"].Value.ShouldBe("CarPlural");
        }
    }

    [Fact]
    public async Task Should_Create_External_Resource_By_Name_Async()
    {
        using (CultureHelper.Use("en"))
        {
            var localizer = await _factory.CreateByResourceNameOrNullAsync(TestExternalLocalizationStore.TestExternalResourceNames.ExternalResource1);
            localizer.ShouldNotBeNull();
            localizer["CarPlural"].Value.ShouldBe("CarPlural");
            
            var localizer2 = await _factory.CreateByResourceNameOrNullAsync(TestExternalLocalizationStore.TestExternalResourceNames.ExternalResource2);
            localizer2.ShouldNotBeNull();
            localizer2["CarPlural"].Value.ShouldBe("CarPlural");
        }
    }

    [Fact]
    public void Should_Create_Resource_By_Name_From_Split_Files()
    {
        using (CultureHelper.Use("en"))
        {
            var localizer = _factory.CreateByResourceNameOrNull("LocalizationTestFilesSplit");
            localizer.ShouldNotBeNull();
            localizer["Base.Id"].Value.ShouldBe("Id");
            localizer["Book.Id"].Value.ShouldBe("ISBN");
            localizer["ThisIsRequiredValue"].Value.ShouldBe("This is required value (this will be used)");
        }
    }

    [Fact]
    public void Should_Get_All_Strings_From_Split_Files()
    {
        using (CultureHelper.Use("en"))
        {
            var localizer = _factory.CreateByResourceNameOrNull("LocalizationTestFilesSplit");
            localizer.ShouldNotBeNull();

            var allStrings = localizer.GetAllStrings(includeParentCultures: false).ToList();

            allStrings.ShouldContain(ls => ls.Name == "Base.Id" && ls.Value == "Id");
            allStrings.ShouldContain(ls => ls.Name == "Base.Name" && ls.Value == "Name");
            allStrings.ShouldContain(ls => ls.Name == "Book.Id" && ls.Value == "ISBN");
            allStrings.ShouldContain(ls => ls.Name == "Book.Name" && ls.Value == "Title");
            allStrings.ShouldContain(ls => ls.Name == "ThisIsRequiredValue" && ls.Value == "This is required value (this will be used)");
        }
    }

    [Fact]
    public void Should_Override_Value_From_Later_Split_File()
    {
        using (CultureHelper.Use("en"))
        {
            var localizer = _factory.CreateByResourceNameOrNull("LocalizationTestFilesSplit");
            localizer.ShouldNotBeNull();

            // !en_First.json defines "This is required value"
            // zen_Last.json defines "This is required value (this will be used)"
            // zen_Last.json sorts after !en_First.json (ordinal: '!' < 'z'), so its value wins
            localizer["ThisIsRequiredValue"].Value.ShouldBe("This is required value (this will be used)");
        }
    }
}
