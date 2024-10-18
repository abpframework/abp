using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using Shouldly;
using Volo.Abp.Json.SystemTextJson.JsonConverters;
using Volo.Abp.Json.SystemTextJson.Modifiers;
using Volo.Abp.Localization;
using Xunit;

namespace Volo.Abp.Json;

public class AbpDatetimeToEnum_Tests : AbpJsonSystemTextJsonTestBase
{
    [Theory]
    [InlineData("tr", "14.02.2024")]
    [InlineData("en-US", "2/14/2024")]
    [InlineData("en-GB", "14/02/2024")]
    public void Test_Read(string culture, string datetime)
    {
        var options = new JsonSerializerOptions()
        {
            TypeInfoResolver = new DefaultJsonTypeInfoResolver()
            {
                Modifiers =
                {
                    new AbpDateTimeConverterModifier(
                            GetRequiredService<AbpDateTimeConverter>(),
                            GetRequiredService<AbpNullableDateTimeConverter>())
                        .CreateModifyAction()
                }
            }
        };

        using(CultureHelper.Use(culture))
        {
            var testClass = JsonSerializer.Deserialize<TestClass>($"{{\"DateTime\": \"{datetime}\", \"NullableDateTime\": \"{datetime}\"}}", options);
            testClass.ShouldNotBeNull();
            testClass.DateTime.ToString(CultureInfo.CurrentCulture).ShouldStartWith(datetime);
            testClass.NullableDateTime.ShouldNotBeNull();
            testClass.NullableDateTime.Value.ToString(CultureInfo.CurrentCulture).ShouldStartWith(datetime);
        }

        using(CultureHelper.Use(culture))
        {
            var testClass = JsonSerializer.Deserialize<TestClass>($"{{\"DateTime\": \"{datetime}\", \"NullableDateTime\": null}}", options);
            testClass.ShouldNotBeNull();
            testClass.DateTime.ToString(CultureInfo.CurrentCulture).ShouldStartWith(datetime);
            testClass.NullableDateTime.ShouldBeNull();
        }
    }

    class TestClass
    {
        public DateTime DateTime { get; set; }

        public DateTime? NullableDateTime { get; set; }
    }
}
