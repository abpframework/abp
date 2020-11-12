using Shouldly;
using Volo.Abp.Data;
using Volo.Abp.Json.SystemTextJson;
using Volo.Abp.ObjectExtending;
using Xunit;

namespace Volo.Abp.Json
{
    public class AbpSystemTextJsonSerializerProvider_Tests : AbpJsonTestBase
    {
        private readonly AbpSystemTextJsonSerializerProvider _jsonSerializer;

        public AbpSystemTextJsonSerializerProvider_Tests()
        {
            _jsonSerializer = GetRequiredService<AbpSystemTextJsonSerializerProvider>();
        }

        [Fact]
        public void Serialize_Deserialize_With_Boolean()
        {
            var json = "{\"name\":\"abp\",\"IsDeleted\":\"fAlSe\"}";
            var file = _jsonSerializer.Deserialize<FileWithBoolean>(json);
            file.Name.ShouldBe("abp");
            file.IsDeleted.ShouldBeFalse();

            file.IsDeleted = false;
            var newJson = _jsonSerializer.Serialize(file);
            newJson.ShouldBe("{\"name\":\"abp\",\"isDeleted\":false}");
        }

        [Fact]
        public void Serialize_Deserialize_With_Nullable_Boolean()
        {
            var json = "{\"name\":\"abp\",\"IsDeleted\":null}";
            var file = _jsonSerializer.Deserialize<FileWithNullableBoolean>(json);
            file.Name.ShouldBe("abp");
            file.IsDeleted.ShouldBeNull();

            var newJson = _jsonSerializer.Serialize(file);
            newJson.ShouldBe("{\"name\":\"abp\",\"isDeleted\":null}");

            json = "{\"name\":\"abp\",\"IsDeleted\":\"true\"}";
            file = _jsonSerializer.Deserialize<FileWithNullableBoolean>(json);
            file.IsDeleted.ShouldNotBeNull();
            file.IsDeleted.Value.ShouldBeTrue();

            newJson = _jsonSerializer.Serialize(file);
            newJson.ShouldBe("{\"name\":\"abp\",\"isDeleted\":true}");
        }

        [Fact]
        public void Serialize_Deserialize_With_Enum()
        {
            var json = "{\"name\":\"abp\",\"type\":\"Exe\"}";
            var file = _jsonSerializer.Deserialize<FileWithEnum>(json);
            file.Name.ShouldBe("abp");
            file.Type.ShouldBe(FileType.Exe);

            var newJson = _jsonSerializer.Serialize(file);
            newJson.ShouldBe("{\"name\":\"abp\",\"type\":2}");
        }

        [Fact]
        public void Serialize_Deserialize_With_Nullable_Enum()
        {
            var json = "{\"name\":\"abp\",\"type\":null}";
            var file = _jsonSerializer.Deserialize<FileWithNullableEnum>(json);
            file.Name.ShouldBe("abp");
            file.Type.ShouldBeNull();

            var newJson = _jsonSerializer.Serialize(file);
            newJson.ShouldBe("{\"name\":\"abp\",\"type\":null}");

            json = "{\"name\":\"abp\",\"type\":\"Exe\"}";
            file = _jsonSerializer.Deserialize<FileWithNullableEnum>(json);
            file.Type.ShouldNotBeNull();
            file.Type.ShouldBe(FileType.Exe);

            newJson = _jsonSerializer.Serialize(file);
            newJson.ShouldBe("{\"name\":\"abp\",\"type\":2}");
        }


        [Fact]
        public void Serialize_Deserialize_ExtensibleObject()
        {
            var json = "{\"name\":\"test\",\"extraProperties\":{\"One\":\"123\",\"Two\":456}}";
            var extensibleObject = _jsonSerializer.Deserialize<TestExtensibleObjectClass>(json);
            extensibleObject.GetProperty("One").ShouldBe("123");
            extensibleObject.GetProperty("Two").ShouldBe(456);

            var newJson = _jsonSerializer.Serialize(extensibleObject);
            newJson.ShouldBe(json);
        }

        class TestExtensibleObjectClass : ExtensibleObject
        {
            public string Name { get; set; }
        }

        class FileWithBoolean
        {
            public string Name { get; set; }

            public bool IsDeleted { get; set; }
        }

        class FileWithNullableBoolean
        {
            public string Name { get; set; }

            public bool? IsDeleted { get; set; }
        }

        class FileWithEnum
        {
            public string Name { get; set; }

            public FileType Type { get; set; }
        }

        class FileWithNullableEnum
        {
            public string Name { get; set; }

            public FileType? Type { get; set; }
        }

        enum FileType
        {
            Zip = 0,
            Exe = 2
        }
    }
}
