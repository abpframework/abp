using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Content;
using Volo.Abp.Http.Modeling;
using Xunit;

namespace Volo.Abp.Http.Modeling;

public class ReturnValueApiDescriptionModel_Tests
{
    [Fact]
    public void Create_Without_ContentTypes_Should_Leave_Property_Null()
    {
        var model = ReturnValueApiDescriptionModel.Create(typeof(string));

        model.ShouldNotBeNull();
        model.TypeSimple.ShouldBe("string");
        model.ContentTypes.ShouldBeNull();
    }

    [Fact]
    public void Create_With_ContentTypes_Should_Populate_The_Property()
    {
        var model = ReturnValueApiDescriptionModel.Create(
            typeof(string),
            new[] { "application/json", "text/plain" });

        model.ContentTypes.ShouldNotBeNull();
        model.ContentTypes!.ShouldBe(new[] { "application/json", "text/plain" });
    }
}

public class ReturnValueApiDescriptionModel_IsRemoteStream_Tests
{
    [Fact]
    public void Direct_IRemoteStreamContent_Should_Be_True()
    {
        var model = ReturnValueApiDescriptionModel.Create(typeof(IRemoteStreamContent));
        model.IsRemoteStream.ShouldBeTrue();
    }

    [Fact]
    public void Concrete_RemoteStreamContent_Should_Be_True()
    {
        var model = ReturnValueApiDescriptionModel.Create(typeof(RemoteStreamContent));
        model.IsRemoteStream.ShouldBeTrue();
    }

    [Fact]
    public void Custom_Subclass_Of_IRemoteStreamContent_Should_Be_False()
    {
        var model = ReturnValueApiDescriptionModel.Create(typeof(MyCustomStreamContent));
        model.IsRemoteStream.ShouldBeFalse();
    }

    [Fact]
    public void Task_Of_IRemoteStreamContent_Should_Be_True_After_UnwrapTask()
    {
        var model = ReturnValueApiDescriptionModel.Create(typeof(Task<IRemoteStreamContent>));
        model.IsRemoteStream.ShouldBeTrue();
    }

    [Fact]
    public void Task_Of_Custom_Stream_Subclass_Should_Be_False_After_UnwrapTask()
    {
        var model = ReturnValueApiDescriptionModel.Create(typeof(Task<MyCustomStreamContent>));
        model.IsRemoteStream.ShouldBeFalse();
    }

    [Fact]
    public void IRemoteStreamContent_Array_Should_Be_False()
    {
        var model = ReturnValueApiDescriptionModel.Create(typeof(IRemoteStreamContent[]));
        model.IsRemoteStream.ShouldBeFalse();
    }

    [Fact]
    public void Concrete_RemoteStreamContent_Array_Should_Be_False()
    {
        var model = ReturnValueApiDescriptionModel.Create(typeof(RemoteStreamContent[]));
        model.IsRemoteStream.ShouldBeFalse();
    }

    [Fact]
    public void List_Of_IRemoteStreamContent_Should_Be_False()
    {
        var model = ReturnValueApiDescriptionModel.Create(typeof(List<IRemoteStreamContent>));
        model.IsRemoteStream.ShouldBeFalse();
    }

    [Fact]
    public void IEnumerable_Of_IRemoteStreamContent_Should_Be_False()
    {
        var model = ReturnValueApiDescriptionModel.Create(typeof(IEnumerable<IRemoteStreamContent>));
        model.IsRemoteStream.ShouldBeFalse();
    }

    [Fact]
    public void IReadOnlyCollection_Of_IRemoteStreamContent_Should_Be_False()
    {
        var model = ReturnValueApiDescriptionModel.Create(typeof(IReadOnlyCollection<IRemoteStreamContent>));
        model.IsRemoteStream.ShouldBeFalse();
    }

    [Fact]
    public void Plain_String_Should_Be_False()
    {
        var model = ReturnValueApiDescriptionModel.Create(typeof(string));
        model.IsRemoteStream.ShouldBeFalse();
    }

    [Fact]
    public void Int_Should_Be_False()
    {
        var model = ReturnValueApiDescriptionModel.Create(typeof(int));
        model.IsRemoteStream.ShouldBeFalse();
    }

    [Fact]
    public void Plain_Dto_Should_Be_False()
    {
        var model = ReturnValueApiDescriptionModel.Create(typeof(PlainDto));
        model.IsRemoteStream.ShouldBeFalse();
    }

    [Fact]
    public void Dto_Containing_IRemoteStreamContent_Property_Should_Be_False()
    {
        var model = ReturnValueApiDescriptionModel.Create(typeof(DtoWithStream));
        model.IsRemoteStream.ShouldBeFalse();
    }

    [Fact]
    public void Dto_Inheriting_From_Type_With_Stream_Should_Be_False()
    {
        var model = ReturnValueApiDescriptionModel.Create(typeof(DtoInheritingStream));
        model.IsRemoteStream.ShouldBeFalse();
    }

    [Fact]
    public void Byte_Array_Should_Be_False()
    {
        var model = ReturnValueApiDescriptionModel.Create(typeof(byte[]));
        model.IsRemoteStream.ShouldBeFalse();
    }

    [Fact]
    public void Dictionary_Should_Be_False()
    {
        var model = ReturnValueApiDescriptionModel.Create(typeof(Dictionary<string, int>));
        model.IsRemoteStream.ShouldBeFalse();
    }

    private class MyCustomStreamContent : IRemoteStreamContent
    {
        public string? FileName => null;
        public string? ContentType => null;
        public long? ContentLength => null;
        public Stream GetStream() => Stream.Null;
        public void Dispose() { }
    }

    private class PlainDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    private class DtoWithStream
    {
        public string FileName { get; set; } = string.Empty;
        public IRemoteStreamContent? File { get; set; }
    }

    private class DtoInheritingStream : PlainDto
    {
        public IRemoteStreamContent? Stream { get; set; }
    }
}

public class ReturnValueApiDescriptionModel_BackwardsCompat_Tests
{
    [Fact]
    public void Deserializing_Json_Without_ContentTypes_Field_Should_Leave_It_Null()
    {
        var json = """
        {
          "type": "System.String",
          "typeSimple": "string"
        }
        """;

        var model = System.Text.Json.JsonSerializer.Deserialize<ReturnValueApiDescriptionModel>(
            json,
            new System.Text.Json.JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        model.ShouldNotBeNull();
        model!.Type.ShouldBe("System.String");
        model.TypeSimple.ShouldBe("string");
        model.ContentTypes.ShouldBeNull();
    }

    [Fact]
    public void Deserializing_Json_With_ContentTypes_Field_Should_Populate_It()
    {
        var json = """
        {
          "type": "System.String",
          "typeSimple": "string",
          "contentTypes": ["application/json", "text/plain"]
        }
        """;

        var model = System.Text.Json.JsonSerializer.Deserialize<ReturnValueApiDescriptionModel>(
            json,
            new System.Text.Json.JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        model!.ContentTypes.ShouldNotBeNull();
        model.ContentTypes!.ShouldBe(new[] { "application/json", "text/plain" });
    }

    [Fact]
    public void Serializing_With_Null_ContentTypes_Should_Emit_Null_Or_Omit()
    {
        var model = ReturnValueApiDescriptionModel.Create(typeof(string));
        var json = System.Text.Json.JsonSerializer.Serialize(model);

        var deserialized = System.Text.Json.JsonSerializer.Deserialize<ReturnValueApiDescriptionModel>(
            json,
            new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        deserialized!.ContentTypes.ShouldBeNull();
    }
}

public class ActionApiDescriptionModel_Tests
{
    [Fact]
    public void Create_Should_Propagate_ReturnValueContentTypes()
    {
        var method = typeof(ActionApiDescriptionModel_Tests).GetMethod(nameof(SampleMethod))!;
        var model = ActionApiDescriptionModel.Create(
            uniqueName: "SampleMethod",
            method: method,
            url: "api/test/sample",
            httpMethod: "GET",
            supportedVersions: new[] { "1.0" },
            allowAnonymous: true,
            authorizeDatas: null,
            implementFrom: null,
            returnValueContentTypes: new[] { "application/octet-stream" });

        model.ReturnValue.ContentTypes.ShouldNotBeNull();
        model.ReturnValue.ContentTypes!.ShouldBe(new[] { "application/octet-stream" });
    }

    [Fact]
    public void Create_Without_ReturnValueContentTypes_Should_Leave_Null()
    {
        var method = typeof(ActionApiDescriptionModel_Tests).GetMethod(nameof(SampleMethod))!;
        var model = ActionApiDescriptionModel.Create(
            uniqueName: "SampleMethod",
            method: method,
            url: "api/test/sample",
            httpMethod: "GET",
            supportedVersions: new[] { "1.0" });

        model.ReturnValue.ContentTypes.ShouldBeNull();
    }

    public string SampleMethod() => string.Empty;
}
