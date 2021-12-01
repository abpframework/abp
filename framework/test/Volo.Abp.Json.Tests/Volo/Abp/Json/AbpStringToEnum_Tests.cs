using System;
using System.Text.Json;
using Shouldly;
using Volo.Abp.Json.SystemTextJson.JsonConverters;
using Xunit;

namespace Volo.Abp.Json
{
    public class AbpStringToEnum_Tests
    {
        [Fact]
        public void Test_Read()
        {
            var options = new JsonSerializerOptions()
            {
                Converters =
                {
                    new AbpStringToEnumFactory()
                }
            };

            var testClass = JsonSerializer.Deserialize<TestClass>("{\"Day\": \"Monday\"}", options);
            testClass.ShouldNotBeNull();
            testClass.Day.ShouldBe(DayOfWeek.Monday);

            testClass = JsonSerializer.Deserialize<TestClass>("{\"Day\": 1}", options);
            testClass.ShouldNotBeNull();
            testClass.Day.ShouldBe(DayOfWeek.Monday);
        }

        [Fact]
        public void Test_Write()
        {
            var options = new JsonSerializerOptions()
            {
                Converters =
                {
                    new AbpStringToEnumFactory()
                }
            };

            var testClassJson = JsonSerializer.Serialize(new TestClass()
            {
                Day = DayOfWeek.Monday
            });

            testClassJson.ShouldBe("{\"Day\":1}");
        }

        class TestClass
        {
            public DayOfWeek Day { get; set; }
        }
    }
}
