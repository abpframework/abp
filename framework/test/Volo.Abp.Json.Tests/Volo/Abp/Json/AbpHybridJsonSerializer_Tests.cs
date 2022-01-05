using System;
using System.Collections.Generic;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Shouldly;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Json.Newtonsoft;
using Volo.Abp.Json.SystemTextJson;
using Xunit;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace Volo.Abp.Json;

public class AbpHybridJsonSerializer_Tests : AbpJsonTestBase
{
    private readonly IJsonSerializer _jsonSerializer;

    public AbpHybridJsonSerializer_Tests()
    {
        _jsonSerializer = GetRequiredService<IJsonSerializer>();
    }

    protected override void AfterAddApplication(IServiceCollection services)
    {
        services.Configure<AbpSystemTextJsonSerializerOptions>(options =>
        {
            options.UnsupportedTypes.Add<MyClass1>();

            options.JsonSerializerOptions.Converters.Add(new SystemTextJsonConverter());
        });

        services.Configure<AbpNewtonsoftJsonSerializerOptions>(options =>
        {
            options.Converters.Add<NewtonsoftJsonConverter>();
        });
    }

    [Fact]
    public void NewtonsoftSerialize_Test()
    {
        var json = _jsonSerializer.Serialize(new MyClass1
        {
            Providers = new List<MyClass3>
                {
                    new MyClass3()
                }
        });

        json.ShouldContain("Newtonsoft");
    }

    [Fact]
    public void SystemTextJsonSerialize_Test()
    {
        var json = _jsonSerializer.Serialize(new MyClass2
        {
            Providers = new List<MyClass3>
                {
                    new MyClass3()
                }
        });

        json.ShouldContain("SystemTextJson");
    }

    [Fact]
    public void SystemTextJsonSerialize_With_Dictionary_Test()
    {
        var json = _jsonSerializer.Serialize(new MyClassWithDictionary
        {
            Properties =
                {
                    {"A", "AV"},
                    {"B", "BV"}
                }
        });

        var deserialized = _jsonSerializer.Deserialize<MyClassWithDictionary>(json);
        deserialized.Properties.ShouldContain(p => p.Key == "A" && p.Value == "AV");
        deserialized.Properties.ShouldContain(p => p.Key == "B" && p.Value == "BV");
    }

    public class MyClass1
    {
        public string Provider { get; set; }

        public List<MyClass3> Providers { get; set; }
    }

    public class MyClass2
    {
        public string Provider { get; set; }

        public List<MyClass3> Providers { get; set; }
    }

    public class MyClass3
    {
        public string Provider { get; set; }
    }

    public class MyClassWithDictionary
    {
        public Dictionary<string, string> Properties { get; set; }

        public MyClassWithDictionary()
        {
            Properties = new Dictionary<string, string>();
        }
    }

    class NewtonsoftJsonConverter : JsonConverter<MyClass1>, ITransientDependency
    {
        public override void WriteJson(JsonWriter writer, MyClass1 value, JsonSerializer serializer)
        {
            value.Provider = "Newtonsoft";
            foreach (var provider in value.Providers)
            {
                provider.Provider = "Newtonsoft";
            }

            writer.WriteRawValue(JsonConvert.SerializeObject(value));
        }

        public override MyClass1 ReadJson(JsonReader reader, Type objectType, MyClass1 existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            return (MyClass1)serializer.Deserialize(reader, objectType);
        }
    }

    class SystemTextJsonConverter : System.Text.Json.Serialization.JsonConverter<MyClass2>, ITransientDependency
    {
        public override void Write(Utf8JsonWriter writer, MyClass2 value, JsonSerializerOptions options)
        {
            value.Provider = "SystemTextJson";
            foreach (var provider in value.Providers)
            {
                provider.Provider = "SystemTextJson";
            }
            System.Text.Json.JsonSerializer.Serialize(writer, value);
        }

        public override MyClass2 Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return (MyClass2)System.Text.Json.JsonSerializer.Deserialize(ref reader, typeToConvert);
        }
    }
}
