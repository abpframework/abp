using System.Text.Json;
using Shouldly;
using Volo.Abp.Json.SystemTextJson.JsonConverters;
using Xunit;

namespace Volo.Abp.Json
{
    public class AbpStringToBoolean_Tests
    {
        [Fact]
        public void Test_Read()
        {
            var options = new JsonSerializerOptions()
            {
                Converters =
                {
                    new AbpStringToBooleanConverter()
                }
            };

            var testClass = JsonSerializer.Deserialize<TestClass>("{\"Enabled\": \"TrUe\"}", options);
            testClass.ShouldNotBeNull();
            testClass.Enabled.ShouldBe(true);

            testClass = JsonSerializer.Deserialize<TestClass>("{\"Enabled\": true}", options);
            testClass.ShouldNotBeNull();
            testClass.Enabled.ShouldBe(true);
        }

        [Fact]
        public void Test_Write()
        {
            var options = new JsonSerializerOptions()
            {
                Converters =
                {
                    new AbpStringToBooleanConverter()
                }
            };

            var testClassJson = JsonSerializer.Serialize(new TestClass()
            {
                Enabled = true
            });

            testClassJson.ShouldBe("{\"Enabled\":true}");
        }

        class TestClass
        {
            public bool Enabled { get; set; }
        }
    }
}
