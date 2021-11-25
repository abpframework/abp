using System;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Data;
using Volo.Abp.Json.SystemTextJson;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Timing;
using Xunit;

namespace Volo.Abp.Json;

public abstract class AbpSystemTextJsonSerializerProvider_TestBase : AbpJsonTestBase
{
    protected AbpSystemTextJsonSerializerProvider JsonSerializer;

    public AbpSystemTextJsonSerializerProvider_TestBase()
    {
        JsonSerializer = GetRequiredService<AbpSystemTextJsonSerializerProvider>();
    }

    public class TestExtensibleObjectClass : ExtensibleObject
    {
        public string Name { get; set; }
    }

    public class FileWithBoolean
    {
        public string Name { get; set; }

        public bool IsDeleted { get; set; }
    }

    public class FileWithNullableBoolean
    {
        public string Name { get; set; }

        public bool? IsDeleted { get; set; }
    }

    public class FileWithEnum
    {
        public string Name { get; set; }

        public FileType Type { get; set; }
    }

    public class FileWithNullableEnum
    {
        public string Name { get; set; }

        public FileType? Type { get; set; }
    }

    public enum FileType
    {
        Zip = 0,
        Exe = 2
    }

    public class FileWithDatetime
    {
        public string Name { get; set; }

        public DateTime CreationTime { get; set; }
    }

    public class FileWithNullableDatetime
    {
        public string Name { get; set; }

        public DateTime? CreationTime { get; set; }
    }
}

public class AbpSystemTextJsonSerializerProvider_Tests : AbpSystemTextJsonSerializerProvider_TestBase
{
    [Fact]
    public void Serialize_Deserialize_With_Boolean()
    {
        var json = "{\"name\":\"abp\",\"IsDeleted\":\"fAlSe\"}";
        var file = JsonSerializer.Deserialize<FileWithBoolean>(json);
        file.Name.ShouldBe("abp");
        file.IsDeleted.ShouldBeFalse();

        file.IsDeleted = false;
        var newJson = JsonSerializer.Serialize(file);
        newJson.ShouldBe("{\"name\":\"abp\",\"isDeleted\":false}");
    }

    [Fact]
    public void Serialize_Deserialize_With_Nullable_Boolean()
    {
        var json = "{\"name\":\"abp\",\"IsDeleted\":null}";
        var file = JsonSerializer.Deserialize<FileWithNullableBoolean>(json);
        file.Name.ShouldBe("abp");
        file.IsDeleted.ShouldBeNull();

        var newJson = JsonSerializer.Serialize(file);
        newJson.ShouldBe("{\"name\":\"abp\",\"isDeleted\":null}");

        json = "{\"name\":\"abp\",\"IsDeleted\":\"true\"}";
        file = JsonSerializer.Deserialize<FileWithNullableBoolean>(json);
        file.IsDeleted.ShouldNotBeNull();
        file.IsDeleted.Value.ShouldBeTrue();

        newJson = JsonSerializer.Serialize(file);
        newJson.ShouldBe("{\"name\":\"abp\",\"isDeleted\":true}");
    }

    [Fact]
    public void Serialize_Deserialize_With_Enum()
    {
        var json = "{\"name\":\"abp\",\"type\":\"Exe\"}";
        var file = JsonSerializer.Deserialize<FileWithEnum>(json);
        file.Name.ShouldBe("abp");
        file.Type.ShouldBe(FileType.Exe);

        var newJson = JsonSerializer.Serialize(file);
        newJson.ShouldBe("{\"name\":\"abp\",\"type\":2}");
    }

    [Fact]
    public void Serialize_Deserialize_With_Nullable_Enum()
    {
        var json = "{\"name\":\"abp\",\"type\":null}";
        var file = JsonSerializer.Deserialize<FileWithNullableEnum>(json);
        file.Name.ShouldBe("abp");
        file.Type.ShouldBeNull();

        var newJson = JsonSerializer.Serialize(file);
        newJson.ShouldBe("{\"name\":\"abp\",\"type\":null}");

        json = "{\"name\":\"abp\",\"type\":\"Exe\"}";
        file = JsonSerializer.Deserialize<FileWithNullableEnum>(json);
        file.Type.ShouldNotBeNull();
        file.Type.ShouldBe(FileType.Exe);

        newJson = JsonSerializer.Serialize(file);
        newJson.ShouldBe("{\"name\":\"abp\",\"type\":2}");
    }

    [Fact]
    public void Serialize_Deserialize_ExtensibleObject()
    {
        var json = "{\"name\":\"test\",\"extraProperties\":{\"One\":\"123\",\"Two\":456}}";
        var extensibleObject = JsonSerializer.Deserialize<TestExtensibleObjectClass>(json);
        extensibleObject.GetProperty("One").ShouldBe("123");
        extensibleObject.GetProperty("Two").ShouldBe(456);

        var newJson = JsonSerializer.Serialize(extensibleObject);
        newJson.ShouldBe(json);
    }

    [Fact]
    public void Serialize_Deserialize_ExtensibleObject_Without_String()
    {
        var json = "{\"name\":\"test\"}";
        var extensibleObject = JsonSerializer.Deserialize<TestExtensibleObjectClass>(json);
        extensibleObject.ExtraProperties.ShouldNotBeNull();
        extensibleObject.ExtraProperties.ShouldBeEmpty();
    }

    [Fact]
    public void Serialize_Deserialize_ExtensibleObject_Without_Empty()
    {
        var json = "{\"name\":\"test\",\"extraProperties\":{}}";
        var extensibleObject = JsonSerializer.Deserialize<TestExtensibleObjectClass>(json);
        extensibleObject.ExtraProperties.ShouldNotBeNull();
        extensibleObject.ExtraProperties.ShouldBeEmpty();
    }

    [Fact]
    public void Serialize_Deserialize_ExtensibleObject_Without_Null()
    {
        var json = "{\"name\":\"test\",\"extraProperties\":null}";
        var extensibleObject = JsonSerializer.Deserialize<TestExtensibleObjectClass>(json);
        extensibleObject.ExtraProperties.ShouldNotBeNull();
        extensibleObject.ExtraProperties.ShouldBeEmpty();
    }

    [Fact]
    public void Serialize_Deserialize_With_Datetime()
    {
        var json = "{\"name\":\"abp\",\"creationTime\":\"2020-11-20T00:00:00\"}";
        var file = JsonSerializer.Deserialize<FileWithDatetime>(json);
        file.CreationTime.Year.ShouldBe(2020);
        file.CreationTime.Month.ShouldBe(11);
        file.CreationTime.Day.ShouldBe(20);

        var newJson = JsonSerializer.Serialize(file);
        newJson.ShouldBe(json);
    }

    [Fact]
    public void Serialize_Deserialize_With_Nullable_Datetime()
    {
        var json = "{\"name\":\"abp\",\"creationTime\":null}";
        var file = JsonSerializer.Deserialize<FileWithNullableDatetime>(json);
        file.CreationTime.ShouldBeNull();

        json = "{\"name\":\"abp\"}";
        file = JsonSerializer.Deserialize<FileWithNullableDatetime>(json);
        file.CreationTime.ShouldBeNull();

        json = "{\"name\":\"abp\",\"creationTime\":\"2020-11-20T00:00:00\"}";
        file = JsonSerializer.Deserialize<FileWithNullableDatetime>(json);
        file.CreationTime.ShouldNotBeNull();

        file.CreationTime.Value.Year.ShouldBe(2020);
        file.CreationTime.Value.Month.ShouldBe(11);
        file.CreationTime.Value.Day.ShouldBe(20);

        var newJson = JsonSerializer.Serialize(file);
        newJson.ShouldBe(json);
    }
}

public class AbpSystemTextJsonSerializerProvider_DateTimeFormat_Tests : AbpSystemTextJsonSerializerProvider_TestBase
{
    protected override void AfterAddApplication(IServiceCollection services)
    {
        services.Configure<AbpJsonOptions>(options =>
        {
            options.DefaultDateTimeFormat = "yyyy*MM*dd";
        });
    }

    [Fact]
    public void Serialize_Deserialize_With_Format_Datetime()
    {
        var json = "{\"name\":\"abp\",\"creationTime\":\"2020*11*20\"}";
        var file = JsonSerializer.Deserialize<FileWithDatetime>(json);
        file.CreationTime.Year.ShouldBe(2020);
        file.CreationTime.Month.ShouldBe(11);
        file.CreationTime.Day.ShouldBe(20);

        var newJson = JsonSerializer.Serialize(file);
        newJson.ShouldBe(json);
    }

    [Fact]
    public void Serialize_Deserialize_With_Nullable_Format_Datetime()
    {
        var json = "{\"name\":\"abp\",\"creationTime\":null}";
        var file = JsonSerializer.Deserialize<FileWithNullableDatetime>(json);
        file.CreationTime.ShouldBeNull();

        json = "{\"name\":\"abp\"}";
        file = JsonSerializer.Deserialize<FileWithNullableDatetime>(json);
        file.CreationTime.ShouldBeNull();

        json = "{\"name\":\"abp\",\"creationTime\":\"2020*11*20\"}";
        file = JsonSerializer.Deserialize<FileWithNullableDatetime>(json);
        file.CreationTime.ShouldNotBeNull();

        file.CreationTime.Value.Year.ShouldBe(2020);
        file.CreationTime.Value.Month.ShouldBe(11);
        file.CreationTime.Value.Day.ShouldBe(20);

        var newJson = JsonSerializer.Serialize(file);
        newJson.ShouldBe(json);
    }
}

public abstract class AbpSystemTextJsonSerializerProvider_Datetime_Kind_Tests : AbpSystemTextJsonSerializerProvider_TestBase
{
    protected DateTimeKind Kind { get; set; } = DateTimeKind.Unspecified;

    [Fact]
    public void Serialize_Deserialize()
    {
        var json = "{\"name\":\"abp\",\"creationTime\":\"2020-11-20T00:00:00\"}";
        var file = JsonSerializer.Deserialize<FileWithDatetime>(json);
        file.CreationTime.Kind.ShouldBe(Kind);
    }
}

public class AbpSystemTextJsonSerializerProvider_Datetime_Kind_UTC_Tests : AbpSystemTextJsonSerializerProvider_Datetime_Kind_Tests
{
    protected override void AfterAddApplication(IServiceCollection services)
    {
        Kind = DateTimeKind.Utc;
        services.Configure<AbpClockOptions>(x => x.Kind = Kind);
    }
}

public class AbpSystemTextJsonSerializerProvider_Datetime_Kind_Local_Tests : AbpSystemTextJsonSerializerProvider_Datetime_Kind_Tests
{
    protected override void AfterAddApplication(IServiceCollection services)
    {
        Kind = DateTimeKind.Local;
        services.Configure<AbpClockOptions>(x => x.Kind = Kind);
    }
}

public class AbpSystemTextJsonSerializerProvider_Datetime_Kind_Unspecified_Tests : AbpSystemTextJsonSerializerProvider_Datetime_Kind_Tests
{

}
