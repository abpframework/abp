using System.Text.Json;
using Shouldly;
using Xunit;

namespace Volo.Abp.Json;

public class ObjectToInferredTypesConverter_Tests : AbpJsonSystemTextJsonTestBase
{
    private readonly IJsonSerializer _jsonSerializer;

    public ObjectToInferredTypesConverter_Tests()
    {
        _jsonSerializer = GetRequiredService<IJsonSerializer>();
    }

    [Fact]
    public void Test()
    {
        var objString = _jsonSerializer.Serialize(new object());
        objString.ShouldBe("{}");
        var obj = _jsonSerializer.Deserialize<object>(objString);
        obj.ShouldBeOfType<JsonElement>();

        var booleanString = _jsonSerializer.Serialize(true);
        booleanString.ShouldBe("true");
        var boolean = _jsonSerializer.Deserialize<bool>(booleanString);
        boolean.ShouldBe(true);

        var booleanString2 = _jsonSerializer.Serialize(false);
        booleanString2.ShouldBe("false");
        var boolean2 = _jsonSerializer.Deserialize<bool>(booleanString2);
        boolean2.ShouldBe(false);

        var numberString = _jsonSerializer.Serialize(1);
        numberString.ShouldBe("1");
        var number = _jsonSerializer.Deserialize<long>(numberString);
        number.ShouldBe(1);

        var numberString2 = _jsonSerializer.Serialize(1.1);
        numberString2.ShouldBe("1.1");
        var number2 = _jsonSerializer.Deserialize<double>(numberString2);
        number2.ShouldBe(1.1);

        var dateString = _jsonSerializer.Serialize(System.DateTime.Parse("2024-01-01"));
        dateString.ShouldBe("\"2024-01-01T00:00:00\"");
        var date = _jsonSerializer.Deserialize<System.DateTime>(dateString);
        date.ShouldBe(System.DateTime.Parse("2024-01-01"));

        var textString = _jsonSerializer.Serialize("text");
        textString.ShouldBe("\"text\"");
        var text = _jsonSerializer.Deserialize<string>(textString);
        text.ShouldBe("text");
    }
}
