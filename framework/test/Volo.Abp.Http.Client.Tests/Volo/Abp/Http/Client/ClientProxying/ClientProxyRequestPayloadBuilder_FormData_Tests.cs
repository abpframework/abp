#nullable enable
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Content;
using Volo.Abp.Http.Modeling;
using Volo.Abp.Http.ProxyScripting.Generators;
using Volo.Abp.Json;
using Volo.Abp.Timing;
using Xunit;
using MicrosoftOptions = Microsoft.Extensions.Options.Options;

namespace Volo.Abp.Http.Client.ClientProxying;

public class ClientProxyRequestPayloadBuilder_FormData_Tests
{
    private readonly ClientProxyRequestPayloadBuilder _builder;
    private readonly IJsonSerializer _jsonSerializer = new StubJsonSerializer();
    private static readonly ApiVersionInfo NoApiVersion = new("Query", "1.0");

    public ClientProxyRequestPayloadBuilder_FormData_Tests()
    {
        var services = new ServiceCollection();
        var scopeFactory = services.BuildServiceProvider().GetRequiredService<IServiceScopeFactory>();
        var options = MicrosoftOptions.Create(new AbpHttpClientProxyingOptions());
        _builder = new ClientProxyRequestPayloadBuilder(scopeFactory, options, new TestClock());
    }

    [Fact]
    public async Task Direct_IRemoteStreamContent_Param_Should_Produce_Single_StreamContent_Part()
    {
        var action = BuildAction(parameters: new[]
        {
            FormFileParam(name: "file", nameOnMethod: "file"),
        });
        var stream = MakeStream("hello-direct");
        var args = new Dictionary<string, object?>
        {
            ["file"] = new RemoteStreamContent(stream, "demo.txt", "text/plain"),
        };

        var content = await InvokeAsync(action, args);

        var multipart = content.ShouldBeOfType<MultipartFormDataContent>();
        var parts = multipart.ToList();
        parts.Count.ShouldBe(1);
        parts[0].Headers.ContentType!.MediaType.ShouldBe("text/plain");
        (await parts[0].ReadAsStringAsync()).ShouldBe("hello-direct");
    }

    [Fact]
    public async Task Dto_With_IRemoteStreamContent_Property_Should_Flatten_To_Name_Plus_File_Parts()
    {
        var action = BuildAction(parameters: new[]
        {
            FormParam(name: "Name", nameOnMethod: "input"),
            FormFileParam(name: "File", nameOnMethod: "input"),
        });
        var args = new Dictionary<string, object?>
        {
            ["input"] = new TestUploadDto
            {
                Name = "Alice",
                File = new RemoteStreamContent(MakeStream("hello-single"), "single.txt", "text/plain"),
            },
        };

        var content = await InvokeAsync(action, args);

        var multipart = content.ShouldBeOfType<MultipartFormDataContent>();
        var parts = multipart.ToList();
        parts.Count.ShouldBe(2);
        await AssertStringPart(parts[0], "Name", "Alice");
        await AssertStreamPart(parts[1], "File", "hello-single", "text/plain");
    }

    [Fact]
    public async Task Dto_With_IEnumerable_IRemoteStreamContent_Should_Emit_One_Part_Per_Stream()
    {
        var action = BuildAction(parameters: new[]
        {
            FormParam(name: "Label", nameOnMethod: "input"),
            FormFileParam(name: "Files", nameOnMethod: "input"),
        });
        var args = new Dictionary<string, object?>
        {
            ["input"] = new TestUploadFilesDto
            {
                Label = "batch",
                Files = new[]
                {
                    new RemoteStreamContent(MakeStream("a-content"), "a.txt", "text/plain"),
                    new RemoteStreamContent(MakeStream("b-content"), "b.txt", "text/csv"),
                },
            },
        };

        var content = await InvokeAsync(action, args);

        var parts = content.ShouldBeOfType<MultipartFormDataContent>().ToList();
        parts.Count.ShouldBe(3);
        await AssertStringPart(parts[0], "Label", "batch");
        await AssertStreamPart(parts[1], "Files", "a-content", "text/plain");
        await AssertStreamPart(parts[2], "Files", "b-content", "text/csv");
    }

    [Fact]
    public async Task Nested_Dto_With_Child_File_Path_Should_Be_Reflected_Via_Dotted_Name()
    {
        var action = BuildAction(parameters: new[]
        {
            FormParam(name: "Outer", nameOnMethod: "input"),
            FormFileParam(name: "Child.File", nameOnMethod: "input"),
        });
        var args = new Dictionary<string, object?>
        {
            ["input"] = new TestNestedUploadDto
            {
                Outer = "outerVal",
                Child = new TestNestedChildDto
                {
                    File = new RemoteStreamContent(MakeStream("hello-nested"), "nested.txt", "text/plain"),
                },
            },
        };

        var content = await InvokeAsync(action, args);

        var parts = content.ShouldBeOfType<MultipartFormDataContent>().ToList();
        parts.Count.ShouldBe(2);
        await AssertStringPart(parts[0], "Outer", "outerVal");
        await AssertStreamPart(parts[1], "Child.File", "hello-nested", "text/plain");
    }

    [Fact]
    public async Task Form_Only_Action_Without_FormFile_Should_Still_Produce_Multipart_With_String_Parts()
    {
        var action = BuildAction(parameters: new[]
        {
            FormParam(name: "Name", nameOnMethod: "input"),
            FormParam(name: "Tag",  nameOnMethod: "input"),
        });
        var args = new Dictionary<string, object?>
        {
            ["input"] = new TestUploadDto { Name = "Alice", Tag = "T1" },
        };

        var content = await InvokeAsync(action, args);

        var parts = content.ShouldBeOfType<MultipartFormDataContent>().ToList();
        parts.Count.ShouldBe(2);
        await AssertStringPart(parts[0], "Name", "Alice");
        await AssertStringPart(parts[1], "Tag",  "T1");
    }

    [Fact]
    public async Task Body_Binding_Wins_Over_Form_And_Returns_Json_StringContent()
    {
        var action = BuildAction(parameters: new[]
        {
            new ParameterApiDescriptionModel
            {
                Name = "input",
                NameOnMethod = "input",
                Type = typeof(TestUploadDto).FullName!,
                TypeSimple = "dto",
                BindingSourceId = ParameterBindingSources.Body,
            },
        });
        var args = new Dictionary<string, object?>
        {
            ["input"] = new TestUploadDto { Name = "Alice" },
        };

        var content = await InvokeAsync(action, args);

        var stringContent = content.ShouldBeOfType<StringContent>();
        stringContent.Headers.ContentType!.MediaType.ShouldBe("application/json");
        var body = await stringContent.ReadAsStringAsync();
        body.ShouldContain("\"Name\":\"Alice\"");
    }

    [Fact]
    public async Task No_Form_Or_Body_Params_Should_Return_Null_Content()
    {
        var action = BuildAction(parameters: new[]
        {
            new ParameterApiDescriptionModel
            {
                Name = "id",
                NameOnMethod = "id",
                Type = "System.Int32",
                TypeSimple = "int",
                BindingSourceId = ParameterBindingSources.Path,
            },
        });
        var args = new Dictionary<string, object?> { ["id"] = 42 };

        var content = await InvokeAsync(action, args);

        content.ShouldBeNull();
    }

    [Fact]
    public async Task FormFile_With_Null_Value_Should_Be_Skipped_Not_Throw()
    {
        var action = BuildAction(parameters: new[]
        {
            FormParam(name: "Name", nameOnMethod: "input"),
            FormFileParam(name: "File", nameOnMethod: "input"),
        });
        var args = new Dictionary<string, object?>
        {
            ["input"] = new TestUploadDto { Name = "Alice", File = null },
        };

        var content = await InvokeAsync(action, args);

        var parts = content.ShouldBeOfType<MultipartFormDataContent>().ToList();
        parts.Count.ShouldBe(1);
        await AssertStringPart(parts[0], "Name", "Alice");
    }

    [Fact]
    public async Task Three_Level_Nested_Dto_Should_Resolve_Outer_Inner_File_Via_Dotted_Path()
    {
        var action = BuildAction(parameters: new[]
        {
            FormParam(name: "Outer", nameOnMethod: "input"),
            FormFileParam(name: "Inner.Child.File", nameOnMethod: "input"),
        });
        var args = new Dictionary<string, object?>
        {
            ["input"] = new TestThreeLevelDto
            {
                Outer = "outerVal",
                Inner = new TestThreeLevelMiddleDto
                {
                    Child = new TestNestedChildDto
                    {
                        File = new RemoteStreamContent(MakeStream("hello-3-levels"), "deep.txt", "text/plain"),
                    },
                },
            },
        };

        var content = await InvokeAsync(action, args);

        var parts = content.ShouldBeOfType<MultipartFormDataContent>().ToList();
        parts.Count.ShouldBe(2);
        await AssertStringPart(parts[0], "Outer", "outerVal");
        await AssertStreamPart(parts[1], "Inner.Child.File", "hello-3-levels", "text/plain");
    }

    [Fact]
    public async Task File_With_UTF8_FileName_Should_Survive_To_Content_Disposition_FileName_Star()
    {
        var action = BuildAction(parameters: new[]
        {
            FormFileParam(name: "file", nameOnMethod: "file"),
        });
        var args = new Dictionary<string, object?>
        {
            ["file"] = new RemoteStreamContent(MakeStream("hello-utf8"), "中文-文件名.txt", "text/plain"),
        };

        var content = await InvokeAsync(action, args);

        var part = content.ShouldBeOfType<MultipartFormDataContent>().Single();
        var disposition = part.Headers.ContentDisposition;
        disposition.ShouldNotBeNull();
        disposition!.FileNameStar.ShouldBe("中文-文件名.txt");
        (await part.ReadAsStringAsync()).ShouldBe("hello-utf8");
    }

    [Fact]
    public async Task Dto_Treated_As_Body_When_Not_Registered_Should_Serialize_As_Json_With_File_Field_Embedded()
    {
        var action = BuildAction(parameters: new[]
        {
            new ParameterApiDescriptionModel
            {
                Name = "input",
                NameOnMethod = "input",
                Type = typeof(TestUploadDto).FullName!,
                TypeSimple = "dto",
                BindingSourceId = ParameterBindingSources.Body,
            },
        });
        var args = new Dictionary<string, object?>
        {
            ["input"] = new TestUploadDto
            {
                Name = "Alice",
                File = new RemoteStreamContent(MakeStream("ignored"), "x.txt", "text/plain"),
            },
        };

        var content = await InvokeAsync(action, args);

        var stringContent = content.ShouldBeOfType<StringContent>();
        stringContent.Headers.ContentType!.MediaType.ShouldBe("application/json");
        var body = await stringContent.ReadAsStringAsync();
        body.ShouldContain("\"Name\":\"Alice\"");
        body.ShouldContain("\"File\"");
    }

    [Fact]
    public async Task Dto_With_Both_Stream_Property_And_Stream_Collection_Should_Emit_All_Parts()
    {
        var action = BuildAction(parameters: new[]
        {
            FormParam(name: "Label", nameOnMethod: "input"),
            FormFileParam(name: "Main", nameOnMethod: "input"),
            FormFileParam(name: "Extras", nameOnMethod: "input"),
        });
        var args = new Dictionary<string, object?>
        {
            ["input"] = new TestMixedUploadDto
            {
                Label = "combo",
                Main = new RemoteStreamContent(MakeStream("main-body"), "main.txt", "text/plain"),
                Extras = new[]
                {
                    new RemoteStreamContent(MakeStream("extra-a"), "ea.txt", "text/csv"),
                    new RemoteStreamContent(MakeStream("extra-b"), "eb.txt", "text/plain"),
                },
            },
        };

        var content = await InvokeAsync(action, args);

        var parts = content.ShouldBeOfType<MultipartFormDataContent>().ToList();
        parts.Count.ShouldBe(4);
        await AssertStringPart(parts[0], "Label", "combo");
        await AssertStreamPart(parts[1], "Main",   "main-body", "text/plain");
        await AssertStreamPart(parts[2], "Extras", "extra-a",   "text/csv");
        await AssertStreamPart(parts[3], "Extras", "extra-b",   "text/plain");
    }

    [Fact]
    public async Task Two_Different_Upload_Actions_On_Same_Builder_Should_Not_Pollute_Each_Other()
    {
        var actionA = BuildAction(parameters: new[]
        {
            FormParam(name: "Name", nameOnMethod: "input"),
            FormFileParam(name: "File", nameOnMethod: "input"),
        });
        var actionB = BuildAction(parameters: new[]
        {
            FormParam(name: "Label", nameOnMethod: "input"),
            FormFileParam(name: "Files", nameOnMethod: "input"),
        });

        var argsA = new Dictionary<string, object?>
        {
            ["input"] = new TestUploadDto { Name = "A", File = new RemoteStreamContent(MakeStream("aa"), "a.txt", "text/plain") },
        };
        var argsB = new Dictionary<string, object?>
        {
            ["input"] = new TestUploadFilesDto
            {
                Label = "B",
                Files = new[] { new RemoteStreamContent(MakeStream("bb"), "b.txt", "text/csv") },
            },
        };

        var contentA = (await InvokeAsync(actionA, argsA)).ShouldBeOfType<MultipartFormDataContent>().ToList();
        var contentB = (await InvokeAsync(actionB, argsB)).ShouldBeOfType<MultipartFormDataContent>().ToList();

        await AssertStringPart(contentA[0], "Name", "A");
        await AssertStreamPart(contentA[1], "File", "aa", "text/plain");
        await AssertStringPart(contentB[0], "Label", "B");
        await AssertStreamPart(contentB[1], "Files", "bb", "text/csv");
    }

    [Fact]
    public async Task Inherited_Dto_With_File_Property_On_Base_Class_Should_Resolve_Via_Reflection()
    {
        var action = BuildAction(parameters: new[]
        {
            FormParam(name: "Name", nameOnMethod: "input"),
            FormFileParam(name: "File", nameOnMethod: "input"),
            FormParam(name: "ChildOnly", nameOnMethod: "input"),
        });
        var args = new Dictionary<string, object?>
        {
            ["input"] = new TestInheritedUploadDto
            {
                Name = "inherited",
                File = new RemoteStreamContent(MakeStream("from-base"), "base.txt", "text/plain"),
                ChildOnly = "extra",
            },
        };

        var content = await InvokeAsync(action, args);

        var parts = content.ShouldBeOfType<MultipartFormDataContent>().ToList();
        parts.Count.ShouldBe(3);
        await AssertStringPart(parts[0], "Name", "inherited");
        await AssertStreamPart(parts[1], "File", "from-base", "text/plain");
        await AssertStringPart(parts[2], "ChildOnly", "extra");
    }

    [Fact]
    public async Task Record_Dto_With_File_Property_Should_Resolve_Via_Reflection()
    {
        var action = BuildAction(parameters: new[]
        {
            FormParam(name: "Name", nameOnMethod: "input"),
            FormFileParam(name: "File", nameOnMethod: "input"),
        });
        var args = new Dictionary<string, object?>
        {
            ["input"] = new TestRecordUploadDto(
                Name: "Diana",
                File: new RemoteStreamContent(MakeStream("from-record"), "r.txt", "text/plain")),
        };

        var content = await InvokeAsync(action, args);

        var parts = content.ShouldBeOfType<MultipartFormDataContent>().ToList();
        parts.Count.ShouldBe(2);
        await AssertStringPart(parts[0], "Name", "Diana");
        await AssertStreamPart(parts[1], "File", "from-record", "text/plain");
    }

    [Fact]
    public async Task Dto_With_DateTime_Enum_And_Nullable_Struct_Fields_Should_Round_Trip_As_String_Parts()
    {
        var action = BuildAction(parameters: new[]
        {
            FormParam(name: "When",     nameOnMethod: "input"),
            FormParam(name: "Status",   nameOnMethod: "input"),
            FormParam(name: "Quantity", nameOnMethod: "input"),
            FormFileParam(name: "File", nameOnMethod: "input"),
        });
        var args = new Dictionary<string, object?>
        {
            ["input"] = new TestPrimitiveUploadDto
            {
                When = new DateTime(2025, 6, 1, 12, 34, 56, DateTimeKind.Utc),
                Status = TestStatus.Active,
                Quantity = 7,
                File = new RemoteStreamContent(MakeStream("primitives"), "p.txt", "text/plain"),
            },
        };

        var content = await InvokeAsync(action, args);

        var parts = content.ShouldBeOfType<MultipartFormDataContent>().ToList();
        parts.Count.ShouldBe(4);
        (await parts[0].ReadAsStringAsync()).ShouldStartWith("2025-06-01T12:34:56");
        (await parts[1].ReadAsStringAsync()).ShouldBe("Active");
        (await parts[2].ReadAsStringAsync()).ShouldBe("7");
        await AssertStreamPart(parts[3], "File", "primitives", "text/plain");
    }

    [Fact]
    public async Task Dto_With_Nullable_Struct_Field_Set_To_Null_Should_Skip_The_Part_Entirely()
    {
        var action = BuildAction(parameters: new[]
        {
            FormParam(name: "Quantity", nameOnMethod: "input"),
            FormFileParam(name: "File", nameOnMethod: "input"),
        });
        var args = new Dictionary<string, object?>
        {
            ["input"] = new TestPrimitiveUploadDto
            {
                Quantity = null,
                File = new RemoteStreamContent(MakeStream("null-qty"), "n.txt", "text/plain"),
            },
        };

        var content = await InvokeAsync(action, args);

        var parts = content.ShouldBeOfType<MultipartFormDataContent>().ToList();
        parts.Count.ShouldBe(1);
        await AssertStreamPart(parts[0], "File", "null-qty", "text/plain");
    }

    [Fact]
    public async Task Polymorphic_Dto_With_Derived_Type_Should_Reflect_Properties_From_Concrete_Runtime_Type()
    {
        var action = BuildAction(parameters: new[]
        {
            FormParam(name: "Name", nameOnMethod: "input"),
            FormParam(name: "ExtraField", nameOnMethod: "input"),
            FormFileParam(name: "File", nameOnMethod: "input"),
        });
        TestUploadDtoBase polymorphic = new TestPolymorphicDerivedDto
        {
            Name = "poly",
            ExtraField = "derived-only",
            File = new RemoteStreamContent(MakeStream("from-derived"), "d.txt", "text/plain"),
        };
        var args = new Dictionary<string, object?> { ["input"] = polymorphic };

        var content = await InvokeAsync(action, args);

        var parts = content.ShouldBeOfType<MultipartFormDataContent>().ToList();
        parts.Count.ShouldBe(3);
        await AssertStringPart(parts[0], "Name", "poly");
        await AssertStringPart(parts[1], "ExtraField", "derived-only");
        await AssertStreamPart(parts[2], "File", "from-derived", "text/plain");
    }

    [Fact]
    public async Task Generic_Dto_Closed_Over_Concrete_Type_Should_Reflect_Open_Generic_Property()
    {
        var action = BuildAction(parameters: new[]
        {
            FormParam(name: "Payload", nameOnMethod: "input"),
            FormFileParam(name: "File", nameOnMethod: "input"),
        });
        var args = new Dictionary<string, object?>
        {
            ["input"] = new TestGenericUploadDto<string>
            {
                Payload = "closed-string",
                File = new RemoteStreamContent(MakeStream("g.txt-body"), "g.txt", "text/plain"),
            },
        };

        var content = await InvokeAsync(action, args);

        var parts = content.ShouldBeOfType<MultipartFormDataContent>().ToList();
        parts.Count.ShouldBe(2);
        await AssertStringPart(parts[0], "Payload", "closed-string");
        await AssertStreamPart(parts[1], "File", "g.txt-body", "text/plain");
    }

    [Fact]
    public async Task Generic_Dto_With_Integer_Payload_Should_Convert_Via_ConvertValueToString()
    {
        var action = BuildAction(parameters: new[]
        {
            FormParam(name: "Payload", nameOnMethod: "input"),
            FormFileParam(name: "File", nameOnMethod: "input"),
        });
        var args = new Dictionary<string, object?>
        {
            ["input"] = new TestGenericUploadDto<int>
            {
                Payload = 42,
                File = new RemoteStreamContent(MakeStream("int-payload"), "i.txt", "text/plain"),
            },
        };

        var content = await InvokeAsync(action, args);

        var parts = content.ShouldBeOfType<MultipartFormDataContent>().ToList();
        parts.Count.ShouldBe(2);
        await AssertStringPart(parts[0], "Payload", "42");
    }

    [Fact]
    public async Task Underlying_Caller_Stream_Is_Wrapped_Without_Buffering_So_Retry_Sees_Drained_Source()
    {
        // Pin pass-through behaviour: HttpClient retry that re-reads `sourceStream`
        // observes an empty body (no internal buffering). Opting in to buffering
        // later would double-allocate for large uploads.
        var action = BuildAction(parameters: new[]
        {
            FormFileParam(name: "file", nameOnMethod: "file"),
        });
        var sourceStream = MakeStream("retry-payload");
        var args = new Dictionary<string, object?>
        {
            ["file"] = new RemoteStreamContent(sourceStream, "r.txt", "text/plain"),
        };

        var content = await InvokeAsync(action, args);

        var part = content.ShouldBeOfType<MultipartFormDataContent>().Single();
        sourceStream.Position.ShouldBe(0);

        (await part.ReadAsStringAsync()).ShouldBe("retry-payload");

        sourceStream.Position.ShouldBe(sourceStream.Length);
    }

    [Fact]
    public async Task Action_With_No_Parameters_Should_Return_Null_Content_Without_Throwing()
    {
        var action = BuildAction(parameters: Array.Empty<ParameterApiDescriptionModel>());
        var args = new Dictionary<string, object?>();

        var content = await InvokeAsync(action, args);

        content.ShouldBeNull();
    }

    [Fact]
    public async Task Path_Plus_Form_Plus_FormFile_Should_Skip_Path_And_Emit_Multipart_Only()
    {
        var action = BuildAction(parameters: new[]
        {
            new ParameterApiDescriptionModel
            {
                Name = "id",
                NameOnMethod = "id",
                Type = "System.Int32",
                TypeSimple = "int",
                BindingSourceId = ParameterBindingSources.Path,
            },
            FormParam(name: "Name", nameOnMethod: "input"),
            FormFileParam(name: "File", nameOnMethod: "input"),
        });
        var args = new Dictionary<string, object?>
        {
            ["id"] = 7,
            ["input"] = new TestUploadDto
            {
                Name = "Bob",
                File = new RemoteStreamContent(MakeStream("path-mixed"), "p.txt", "text/plain"),
            },
        };

        var content = await InvokeAsync(action, args);

        var parts = content.ShouldBeOfType<MultipartFormDataContent>().ToList();
        parts.Count.ShouldBe(2);
        await AssertStringPart(parts[0], "Name", "Bob");
        await AssertStreamPart(parts[1], "File", "path-mixed", "text/plain");
    }

    private Task<HttpContent?> InvokeAsync(ActionApiDescriptionModel action, IReadOnlyDictionary<string, object?> args)
        => _builder.BuildContentAsync(action, args, _jsonSerializer, NoApiVersion);

    private static ActionApiDescriptionModel BuildAction(ParameterApiDescriptionModel[] parameters)
        => new()
        {
            UniqueName = "TestAction",
            Name = "TestAction",
            HttpMethod = "POST",
            Url = "api/test",
            SupportedVersions = new List<string>(),
            ParametersOnMethod = new List<MethodParameterApiDescriptionModel>(),
            Parameters = parameters.ToList(),
            ReturnValue = new ReturnValueApiDescriptionModel
            {
                Type = "System.String",
                TypeSimple = "string",
            },
            AuthorizeDatas = new List<AuthorizeDataApiDescriptionModel>(),
        };

    private static ParameterApiDescriptionModel FormParam(string name, string nameOnMethod)
        => new()
        {
            Name = name,
            NameOnMethod = nameOnMethod,
            Type = "System.String",
            TypeSimple = "string",
            BindingSourceId = ParameterBindingSources.Form,
        };

    private static ParameterApiDescriptionModel FormFileParam(string name, string nameOnMethod)
        => new()
        {
            Name = name,
            NameOnMethod = nameOnMethod,
            Type = typeof(IRemoteStreamContent).FullName!,
            TypeSimple = "stream",
            BindingSourceId = ParameterBindingSources.FormFile,
        };

    private static MemoryStream MakeStream(string text)
    {
        var ms = new MemoryStream();
        ms.Write(Encoding.UTF8.GetBytes(text));
        ms.Position = 0;
        return ms;
    }

    private static async Task AssertStringPart(HttpContent part, string expectedName, string expectedValue)
    {
        var disposition = part.Headers.ContentDisposition;
        disposition.ShouldNotBeNull();
        disposition!.Name!.Trim('"').ShouldBe(expectedName);
        (await part.ReadAsStringAsync()).ShouldBe(expectedValue);
    }

    private static async Task AssertStreamPart(HttpContent part, string expectedName, string expectedBody, string expectedContentType)
    {
        var disposition = part.Headers.ContentDisposition;
        disposition.ShouldNotBeNull();
        disposition!.Name!.Trim('"').ShouldBe(expectedName);
        part.Headers.ContentType!.MediaType.ShouldBe(expectedContentType);
        (await part.ReadAsStringAsync()).ShouldBe(expectedBody);
    }

    private class TestUploadDto
    {
        public string? Name { get; set; }
        public string? Tag { get; set; }
        public IRemoteStreamContent? File { get; set; }
    }

    private class TestUploadFilesDto
    {
        public string? Label { get; set; }
        public IEnumerable<IRemoteStreamContent>? Files { get; set; }
    }

    private class TestNestedUploadDto
    {
        public string? Outer { get; set; }
        public TestNestedChildDto? Child { get; set; }
    }

    private class TestNestedChildDto
    {
        public IRemoteStreamContent? File { get; set; }
    }

    private class TestThreeLevelDto
    {
        public string? Outer { get; set; }
        public TestThreeLevelMiddleDto? Inner { get; set; }
    }

    private class TestThreeLevelMiddleDto
    {
        public TestNestedChildDto? Child { get; set; }
    }

    private class TestMixedUploadDto
    {
        public string? Label { get; set; }
        public IRemoteStreamContent? Main { get; set; }
        public IEnumerable<IRemoteStreamContent>? Extras { get; set; }
    }

    private class TestUploadDtoBase
    {
        public string? Name { get; set; }
        public IRemoteStreamContent? File { get; set; }
    }

    private class TestInheritedUploadDto : TestUploadDtoBase
    {
        public string? ChildOnly { get; set; }
    }

    private record TestRecordUploadDto(string Name, IRemoteStreamContent File);

    private class TestPrimitiveUploadDto
    {
        public DateTime When { get; set; }
        public TestStatus Status { get; set; }
        public int? Quantity { get; set; }
        public IRemoteStreamContent? File { get; set; }
    }

    private enum TestStatus
    {
        Pending = 0,
        Active = 1,
        Done = 2,
    }

    private class TestPolymorphicDerivedDto : TestUploadDtoBase
    {
        public string? ExtraField { get; set; }
    }

    private class TestGenericUploadDto<T>
    {
        public T? Payload { get; set; }
        public IRemoteStreamContent? File { get; set; }
    }

    private class StubJsonSerializer : IJsonSerializer
    {
        public string Serialize(object obj, bool camelCase = true, bool indented = false)
            => System.Text.Json.JsonSerializer.Serialize(obj);

        public T Deserialize<T>(string jsonString, bool camelCase = true)
            => System.Text.Json.JsonSerializer.Deserialize<T>(jsonString)!;

        public object Deserialize(Type type, string jsonString, bool camelCase = true)
            => System.Text.Json.JsonSerializer.Deserialize(jsonString, type)!;
    }

    private class TestClock : IClock
    {
        public DateTime Now => DateTime.UtcNow;
        public DateTimeKind Kind => DateTimeKind.Utc;
        public bool SupportsMultipleTimezone => false;
        public DateTime Normalize(DateTime dateTime) => dateTime;
        public DateTime ConvertToUserTime(DateTime utcDateTime) => utcDateTime;
        public DateTimeOffset ConvertToUserTime(DateTimeOffset dateTimeOffset) => dateTimeOffset;
        public DateTime ConvertToUtc(DateTime dateTime) => dateTime;
    }
}
