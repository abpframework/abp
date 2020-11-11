using Shouldly;
using Volo.Abp.Json.SystemTextJson;
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
        public void Serialize_Deserialize_With_Enum()
        {
            var json = "{\"name\":\"abp\",\"type\":\"Exe\"}";
            var file = _jsonSerializer.Deserialize<FileWithEnum>(json);
            file.Name.ShouldBe("abp");
            file.Type.ShouldBe(FileType.Exe);

            var newJson = _jsonSerializer.Serialize(file);
            newJson.ShouldBe("{\"name\":\"abp\",\"type\":2}");
        }

        class FileWithBoolean
        {
            public string Name { get; set; }

            public bool IsDeleted { get; set; }

        }

        class FileWithEnum
        {
            public string Name { get; set; }

            public FileType Type { get; set; }
        }


        enum FileType
        {
            Zip = 0,
            Exe = 2
        }
    }
}
