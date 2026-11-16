using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using Shouldly;
using Volo.Abp.Http.Modeling;
using Volo.Abp.Json;
using Volo.Abp.Json.SystemTextJson;
using Volo.Abp.VirtualFileSystem;
using Xunit;

namespace Volo.Abp.Http.Client.ClientProxying;

public class ClientProxyApiDescriptionFinder_Tests : AbpHttpClientTestBase
{
    private readonly RecordingJsonSerializer _jsonSerializer;

    public ClientProxyApiDescriptionFinder_Tests()
    {
        _jsonSerializer = new RecordingJsonSerializer(ServiceProvider.GetRequiredService<IJsonSerializer>());
    }

    [Fact]
    public void Should_Deserialize_Only_The_Files_Of_The_Requested_Services()
    {
        var finder = CreateFinder(
            CreateFile("a", CreateContent("a", ("Test.IA", "api/a"))),
            CreateFile("b", CreateContent("b", ("Test.IB", "api/b"))));

        _jsonSerializer.Contents.ShouldBeEmpty();

        finder.FindAction("Test.IB.GetAsync.")!.Url.ShouldBe("api/b");
        _jsonSerializer.Contents.Count.ShouldBe(1);

        finder.FindAction("Test.IB.GetAsync.")!.Url.ShouldBe("api/b");
        finder.FindAction("Test.IA.GetAsync.")!.Url.ShouldBe("api/a");
        _jsonSerializer.Contents.Count.ShouldBe(2);
    }

    [Fact]
    public void Should_Find_The_Other_Services_Of_A_Deserialized_File()
    {
        var finder = CreateFinder(
            CreateFile("a", CreateContent("a", ("Test.IA", "api/a"), ("Test.IB", "api/b"))),
            CreateFile("c", CreateContent("c", ("Test.IC", "api/c"))));

        finder.FindAction("Test.IA.GetAsync.")!.Url.ShouldBe("api/a");
        finder.FindAction("Test.IB.GetAsync.")!.Url.ShouldBe("api/b");
        _jsonSerializer.Contents.Count.ShouldBe(1);
    }

    [Fact]
    public void Should_Return_Null_For_An_Unknown_Action()
    {
        var finder = CreateFinder(
            CreateFile("a", CreateContent("a", ("Test.IA", "api/a"))),
            CreateFile("b", CreateContent("b", ("Test.IB", "api/b"))));

        finder.FindAction("Test.IC.GetAsync.").ShouldBeNull();
        _jsonSerializer.Contents.Count.ShouldBe(2);
    }

    [Fact]
    public void Should_Load_All_Files_If_The_Action_Is_Not_In_The_File_Of_Its_Service()
    {
        var finder = CreateFinder(
            CreateFile("a", CreateContent("a", ("Test.IA", "api/a"))),
            CreateFile("b", CreateContent("b", ("Test.IB", "api/b"))));

        finder.FindAction("Test.IA.DeleteAsync.").ShouldBeNull();
        _jsonSerializer.Contents.Count.ShouldBe(2);
    }

    [Fact]
    public void Should_Keep_The_Action_Of_The_First_File()
    {
        var finder = CreateFinder(
            CreateFile("a", CreateContent("a", ("Test.IA", "api/a"))),
            CreateFile("b", CreateContent("b", ("Test.IA", "api/b"))));

        finder.FindAction("Test.IA.GetAsync.")!.Url.ShouldBe("api/a");
    }

    [Theory]
    [InlineData("\"interfaces\": [", "\"interfaces\": [ /* comment */")]
    [InlineData("\"modules\"", "/* \"interfaces\": [not-json] */ \"modules\"")]
    [InlineData("\"interfaces\"", "\"interf\\u0061ces\"")]
    [InlineData("\"interfaces\"", "\"Interfaces\"")]
    [InlineData("\"type\": \"Test.IA\"", "\"Type\": \"Test.IA\"")]
    [InlineData("\"Test.IA\"", "\"Test\\u002EIA\"")]
    public void Should_Find_The_Service_In_Other_Json_Forms(string oldValue, string newValue)
    {
        var finder = CreateFinder(
            CreateFile("a", CreateContent("a", ("Test.IA", "api/a")).Replace(oldValue, newValue)),
            CreateFile("b", CreateContent("b", ("Test.IB", "api/b"))));

        finder.FindAction("Test.IA.GetAsync.")!.Url.ShouldBe("api/a");
        _jsonSerializer.Contents.Count.ShouldBe(1);
    }

    [Fact]
    public void Should_Find_The_Service_With_The_Options_Of_The_Json_Serializer()
    {
        var jsonSerializerOptions = new AbpSystemTextJsonSerializerOptions();
        jsonSerializerOptions.JsonSerializerOptions.MaxDepth = 128;
        jsonSerializerOptions.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
        var jsonSerializer = new RecordingJsonSerializer(new AbpSystemTextJsonSerializer(Microsoft.Extensions.Options.Options.Create(jsonSerializerOptions)));

        var content = CreateContent("a", ("Test.IA", "api/a"))
            .Replace("\"controllerName\"", "\"extra\": " + new string('[', 70) + new string(']', 70) + ", \"controllerName\"");
        content = Regex.Replace(
            content,
            "\"interfaces\": \\[(.*?)\\],(\\s*)\"actions\"",
            "\"interfaces\": { \"$id\": \"1\", \"$values\": [$1] },$2\"actions\"",
            RegexOptions.Singleline);

        var finder = new ClientProxyApiDescriptionFinder(
            new TestVirtualFileProvider(new[]
            {
                CreateFile("a", content),
                CreateFile("b", CreateContent("b", ("Test.IB", "api/b")))
            }),
            jsonSerializer);

        finder.FindAction("Test.IA.GetAsync.")!.Url.ShouldBe("api/a");
        jsonSerializer.Contents.Count.ShouldBe(1);
    }

    [Fact]
    public void Should_Load_All_Files_If_The_Json_Serializer_Can_Not_Find_The_Services()
    {
        var jsonSerializerOptions = new AbpSystemTextJsonSerializerOptions();
        jsonSerializerOptions.JsonSerializerOptions.UnmappedMemberHandling = JsonUnmappedMemberHandling.Disallow;
        var jsonSerializer = new RecordingJsonSerializer(new AbpSystemTextJsonSerializer(Microsoft.Extensions.Options.Options.Create(jsonSerializerOptions)));

        var finder = new ClientProxyApiDescriptionFinder(
            new TestVirtualFileProvider(new[]
            {
                CreateFile("a", CreateContent("a", ("Test.IA", "api/a"))),
                CreateFile("b", CreateContent("b", ("Test.IB", "api/b")))
            }),
            jsonSerializer);

        finder.FindAction("Test.IB.GetAsync.")!.Url.ShouldBe("api/b");
        jsonSerializer.Contents.Count.ShouldBe(2);

        finder.FindAction("Test.IA.GetAsync.")!.Url.ShouldBe("api/a");
        jsonSerializer.Contents.Count.ShouldBe(2);
    }

    [Fact]
    public void Should_Keep_The_Module_Of_The_File_Deserialized_First()
    {
        var finder = CreateFinder(
            CreateFile("a", CreateContent("app", ("Test.IA", "api/a"))),
            CreateFile("b", CreateContent("app", ("Test.IB", "api/b"))));

        finder.FindAction("Test.IB.GetAsync.")!.Url.ShouldBe("api/b");
        finder.FindAction("Test.IA.GetAsync.").ShouldBeNull();
        finder.GetApiDescription().Modules["app"].Controllers.Keys.ShouldBe(new[] { "Test.app.Controller0" });
    }

    [Fact]
    public void Should_Use_The_Last_Interface_And_The_Parameter_Types_In_The_Action_Key()
    {
        var finder = CreateFinder(
            CreateFile("a", CreateContent("a", ("Test.IBase,Test.IA", "api/a"))
                .Replace("\"parametersOnMethod\": []", "\"parametersOnMethod\": [ { \"name\": \"id\", \"type\": \"System.Guid\" } ]")),
            CreateFile("b", CreateContent("b", ("Test.IB", "api/b"))));

        finder.FindAction("Test.IA.GetAsync.System.Guid")!.Url.ShouldBe("api/a");
        finder.FindAction("Test.IBase.GetAsync.System.Guid").ShouldBeNull();
        finder.FindAction("Test.IA.GetAsync.").ShouldBeNull();
    }

    [Fact]
    public void Should_Throw_Again_If_A_File_Fails_To_Load()
    {
        var finder = CreateFinder(
            CreateFile("a", CreateContent("a", ("Test.IA", "api/a")).Replace("\"parametersOnMethod\": []", "\"parametersOnMethod\": null")),
            CreateFile("b", CreateContent("b", ("Test.IB", "api/b"))));

        Should.Throw<ArgumentNullException>(() => finder.FindAction("Test.IA.GetAsync."));
        Should.Throw<ArgumentNullException>(() => finder.FindAction("Test.IA.GetAsync."));
        finder.FindAction("Test.IB.GetAsync.")!.Url.ShouldBe("api/b");
    }

    [Fact]
    public void GetApiDescription_Should_Return_All_Modules()
    {
        var finder = CreateFinder(
            CreateFile("a", CreateContent("a", ("Test.IA", "api/a"))),
            CreateFile("b", CreateContent("b", ("Test.IB", "api/b"))));

        finder.FindAction("Test.IA.GetAsync.").ShouldNotBeNull();
        finder.GetApiDescription().Modules.Keys.OrderBy(x => x).ShouldBe(new[] { "a", "b" });
        _jsonSerializer.Contents.Count.ShouldBe(2);
    }

    [Fact]
    public async Task Should_Deserialize_Each_File_Once_When_Called_Concurrently()
    {
        var finder = CreateFinder(
            CreateFile("a", CreateContent("a", ("Test.IA", "api/a"))),
            CreateFile("b", CreateContent("b", ("Test.IB", "api/b"))));

        using (var start = new ManualResetEventSlim())
        {
            var tasks = Enumerable.Range(0, 20).Select(i => Task.Run(() =>
            {
                start.Wait();
                if (i % 10 == 0)
                {
                    finder.GetApiDescription().Modules.Count.ShouldBe(2);
                }

                return finder.FindAction(i % 2 == 0 ? "Test.IA.GetAsync." : "Test.IB.GetAsync.");
            })).ToList();

            start.Set();
            (await Task.WhenAll(tasks)).ShouldAllBe(x => x != null);
        }

        _jsonSerializer.Contents.Count.ShouldBe(2);
    }

    [Fact]
    public async Task Should_Deserialize_Each_File_Once_When_Its_Actions_Are_First_Requested_Concurrently()
    {
        _jsonSerializer.DeserializeDelay = TimeSpan.FromMilliseconds(50);
        var finder = CreateFinder(
            CreateFile("a", CreateContent("a", ("Test.IA", "api/a"))),
            CreateFile("b", CreateContent("b", ("Test.IB", "api/b"))),
            CreateFile("c", CreateContent("c", ("Test.IC", "api/c"))));
        var names = new[] { "a", "b", "c" };

        using (var start = new ManualResetEventSlim())
        {
            var tasks = Enumerable.Range(0, 30).Select(i => Task.Run(() =>
            {
                start.Wait();
                var name = names[i % names.Length];
                return (Url: "api/" + name, Action: finder.FindAction($"Test.I{name.ToUpperInvariant()}.GetAsync."));
            })).ToList();

            start.Set();
            (await Task.WhenAll(tasks)).ShouldAllBe(x => x.Action != null && x.Action.Url == x.Url);
        }

        _jsonSerializer.Contents.Count.ShouldBe(3);
    }

    [Fact]
    public void Derived_Finder_Can_Load_All_Files_In_Initialize()
    {
        var finder = new LoadAllFilesClientProxyApiDescriptionFinder(
            new TestVirtualFileProvider(new[]
            {
                CreateFile("a", CreateContent("a", ("Test.IA", "api/a"))),
                CreateFile("b", CreateContent("b", ("Test.IB", "api/b")))
            }),
            _jsonSerializer);

        _jsonSerializer.Contents.Count.ShouldBe(2);
        finder.FindAction("Test.IB.GetAsync.")!.Url.ShouldBe("api/b");
        _jsonSerializer.Contents.Count.ShouldBe(2);
    }

    private ClientProxyApiDescriptionFinder CreateFinder(params IFileInfo[] files)
    {
        return new ClientProxyApiDescriptionFinder(new TestVirtualFileProvider(files), _jsonSerializer);
    }

    private static IFileInfo CreateFile(string name, string content)
    {
        var fileName = name + "-generate-proxy.json";
        return new InMemoryFileInfo("/" + fileName, Encoding.UTF8.GetBytes(content), fileName);
    }

    private static string CreateContent(string rootPath, params (string ServiceType, string Url)[] services)
    {
        var controllers = services.Select((service, index) => new
        {
            controllerName = "Test" + index,
            type = $"Test.{rootPath}.Controller{index}",
            interfaces = service.ServiceType.Split(',').Select(serviceType =>
                new
                {
                    type = serviceType,
                    name = "IService",
                    methods = new[]
                    {
                        new
                        {
                            name = "GetAsync",
                            parametersOnMethod = Array.Empty<object>(),
                            returnValue = new { type = "System.String", typeSimple = "string" }
                        }
                    }
                }).ToArray(),
            actions = new Dictionary<string, object>
            {
                ["GetAsync"] = new
                {
                    uniqueName = "GetAsync",
                    name = "GetAsync",
                    httpMethod = "GET",
                    url = service.Url,
                    parametersOnMethod = Array.Empty<object>(),
                    parameters = Array.Empty<object>()
                }
            }
        });

        return System.Text.Json.JsonSerializer.Serialize(new
        {
            modules = new Dictionary<string, object>
            {
                [rootPath] = new
                {
                    rootPath,
                    remoteServiceName = rootPath,
                    controllers = controllers.ToDictionary(x => x.type, x => (object)x)
                }
            }
        }, new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
    }

    private class LoadAllFilesClientProxyApiDescriptionFinder : ClientProxyApiDescriptionFinder
    {
        public LoadAllFilesClientProxyApiDescriptionFinder(IVirtualFileProvider virtualFileProvider, IJsonSerializer jsonSerializer)
            : base(virtualFileProvider, jsonSerializer)
        {
        }

        protected override void Initialize()
        {
            base.Initialize();
            GetApiDescription();
        }
    }

    private class RecordingJsonSerializer : IJsonSerializer
    {
        private readonly IJsonSerializer _jsonSerializer;
        private readonly ConcurrentQueue<string> _contents = new();

        public IReadOnlyCollection<string> Contents => _contents;

        public TimeSpan DeserializeDelay { get; set; }

        public RecordingJsonSerializer(IJsonSerializer jsonSerializer)
        {
            _jsonSerializer = jsonSerializer;
        }

        public string Serialize(object obj, bool camelCase = true, bool indented = false)
        {
            return _jsonSerializer.Serialize(obj, camelCase, indented);
        }

        public T Deserialize<T>(string jsonString, bool camelCase = true)
        {
            if (typeof(T) == typeof(ApplicationApiDescriptionModel))
            {
                _contents.Enqueue(jsonString);
                Thread.Sleep(DeserializeDelay);
            }

            return _jsonSerializer.Deserialize<T>(jsonString, camelCase);
        }

        public object Deserialize(Type type, string jsonString, bool camelCase = true)
        {
            if (type == typeof(ApplicationApiDescriptionModel))
            {
                _contents.Enqueue(jsonString);
            }

            return _jsonSerializer.Deserialize(type, jsonString, camelCase);
        }
    }

    private class TestVirtualFileProvider : IVirtualFileProvider
    {
        private readonly IFileInfo[] _files;

        public TestVirtualFileProvider(IFileInfo[] files)
        {
            _files = files;
        }

        public IFileInfo GetFileInfo(string subpath)
        {
            return _files.FirstOrDefault(x => "/" + x.Name == subpath) ?? new NotFoundFileInfo(subpath);
        }

        public IDirectoryContents GetDirectoryContents(string subpath)
        {
            return subpath == "" ? new TestDirectoryContents(_files) : NotFoundDirectoryContents.Singleton;
        }

        public IChangeToken Watch(string filter)
        {
            return NullChangeToken.Singleton;
        }
    }

    private class TestDirectoryContents : IDirectoryContents
    {
        private readonly IFileInfo[] _files;

        public bool Exists => true;

        public TestDirectoryContents(IFileInfo[] files)
        {
            _files = files;
        }

        public IEnumerator<IFileInfo> GetEnumerator()
        {
            return ((IEnumerable<IFileInfo>)_files).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
