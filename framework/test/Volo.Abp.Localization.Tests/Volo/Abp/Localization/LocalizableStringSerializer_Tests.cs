using Shouldly;
using Volo.Abp.Localization.TestResources.Source;
using Volo.Abp.Testing;
using Xunit;

namespace Volo.Abp.Localization;

public class LocalizableStringSerializer_Tests : AbpIntegratedTest<AbpLocalizationTestModule>
{
    private readonly ILocalizableStringSerializer _serializer;
    
    public LocalizableStringSerializer_Tests()
    {
        _serializer = GetRequiredService<ILocalizableStringSerializer>();
    }

    [Fact]
    public void Serialize_FixedLocalizableString()
    {
        _serializer
            .Serialize(new FixedLocalizableString(""))
            .ShouldBe("F:");
        
        _serializer
            .Serialize(new FixedLocalizableString("Hello World"))
            .ShouldBe("F:Hello World");
    }
    
    [Fact]
    public void Serialize_LocalizableString()
    {
        _serializer
            .Serialize(new LocalizableString(typeof(LocalizationTestResource),"Car"))
            .ShouldBe("L:Test,Car");
    }
    
    [Fact]
    public void Deserialize_FixedLocalizableString()
    {
        _serializer
            .Deserialize("")
            .ShouldBeOfType<FixedLocalizableString>()
            .Value.ShouldBe("");
        
        _serializer
            .Deserialize("Hello")
            .ShouldBeOfType<FixedLocalizableString>()
            .Value.ShouldBe("Hello");

        _serializer
            .Deserialize("F:Hello")
            .ShouldBeOfType<FixedLocalizableString>()
            .Value.ShouldBe("Hello");
    }
    
    [Fact]
    public void Deserialize_LocalizableString()
    {
        var localizableString = _serializer
            .Deserialize("L:Test,Car")
            .ShouldBeOfType<LocalizableString>();
        localizableString.ResourceType.ShouldBe(typeof(LocalizationTestResource));
        localizableString.Name.ShouldBe("Car");

        Assert.Throws<AbpException>(() =>
        {
            _serializer.Deserialize("L:Test");
        });
        
        Assert.Throws<AbpException>(() =>
        {
            _serializer.Deserialize("L:Test, ");
        });
    }
}