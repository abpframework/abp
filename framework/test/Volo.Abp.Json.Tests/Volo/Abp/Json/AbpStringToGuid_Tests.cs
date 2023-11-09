using System;
using System.Text.Json;
using Shouldly;
using Volo.Abp.Json.SystemTextJson.JsonConverters;
using Xunit;

namespace Volo.Abp.Json;

public class AbpStringToGuid_Tests
{
    [Fact]
    public void Test_Read()
    {
        var options = new JsonSerializerOptions()
        {
            Converters =
            {
                new AbpStringToGuidConverter(),
                new AbpNullableStringToGuidConverter()
            }
        };

        var guid = Guid.Parse("762DDB84-5225-4853-A566-FF0B3AF57585");
        var testClass = JsonSerializer.Deserialize<TestClass>("{" +
                                                              $"\"Id\": \"{guid:N}\", " +
                                                              $"\"NullableId\": \"{guid:D}\", " +
                                                              $"\"NullableId2\": \"{guid:B}\", " +
                                                              $"\"NullableId3\": \"{guid:P}\", " +
                                                              $"\"NullableId4\": \"{guid:X}\", " +
                                                              "\"NullableId5\": \"\", " +
                                                              "\"NullableId6\": null}", options);
        testClass.ShouldNotBeNull();
        testClass.Id.ShouldBe(guid);
        testClass.NullableId.ShouldBe(guid);
        testClass.NullableId2.ShouldBe(guid);
        testClass.NullableId3.ShouldBe(guid);
        testClass.NullableId4.ShouldBe(guid);
        testClass.NullableId5.ShouldBeNull();
        testClass.NullableId6.ShouldBeNull();
    }

    [Fact]
    public void Test_Write()
    {
        var options = new JsonSerializerOptions()
        {
            Converters =
            {
                new AbpStringToGuidConverter(),
                new AbpNullableStringToGuidConverter()
            }
        };

        var guid = Guid.Parse("762DDB84-5225-4853-A566-FF0B3AF57585");
        var json = JsonSerializer.Serialize(new TestClass()
        {
            Id = guid,
            NullableId = null,
            NullableId2 = guid,
            NullableId3 = null,
            NullableId4 = guid,
            NullableId5 = null,
            NullableId6 = guid
        }, options);

        json.ShouldBe($"{{\"Id\":\"{guid:D}\",\"NullableId\":null,\"NullableId2\":\"{guid:D}\",\"NullableId3\":null,\"NullableId4\":\"{guid:D}\",\"NullableId5\":null,\"NullableId6\":\"{guid:D}\"}}");
    }

    class TestClass
    {
        public Guid Id { get; set; }

        public Guid? NullableId { get; set; }

        public Guid? NullableId2 { get; set; }

        public Guid? NullableId3 { get; set; }

        public Guid? NullableId4 { get; set; }

        public Guid? NullableId5 { get; set; }

        public Guid? NullableId6 { get; set; }
    }
}
