using System;
using System.IO;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Cli.Commands;
using Volo.Abp.Cli.Http;
using Volo.Abp.Cli.ServiceProxying.CSharp;
using Volo.Abp.Http.Modeling;
using Volo.Abp.Json;
using Xunit;

namespace Volo.Abp.Cli.ServiceProxying;

public class CSharpServiceProxyGenerator_AsyncEnumerable_Tests : AbpCliTestBase
{
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task Should_Return_IAsyncEnumerable_From_The_Generated_Proxy_And_Interface(bool withoutContracts)
    {
        var workDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(workDirectory);
        await File.WriteAllTextAsync(Path.Combine(workDirectory, "MyProject.HttpApi.Client.csproj"), "<Project Sdk=\"Microsoft.NET.Sdk\" />");
        try
        {
            var generator = new TestCSharpServiceProxyGenerator(GetRequiredService<CliHttpClientFactory>(), GetRequiredService<IJsonSerializer>());

            await generator.GenerateProxyAsync(new GenerateProxyArgs(
                GenerateProxyCommand.Name, workDirectory, "app", "https://localhost/", null, null, null, null, null, null, null, withoutContracts));

            var folder = Path.Combine(workDirectory, "ClientProxies", "MyProject", "Streaming");
            var proxy = await File.ReadAllTextAsync(Path.Combine(folder, "StreamingClientProxy.Generated.cs"));
            proxy.ShouldContain("public virtual IAsyncEnumerable<Int32> GetItemsAsync()");
            proxy.ShouldContain("return RequestAsyncEnumerable<Int32>(nameof(GetItemsAsync));");
            proxy.ShouldContain("public virtual IAsyncEnumerable<String> GetNamedItemsAsync(string name)");
            proxy.ShouldContain("public virtual async Task<String> GetNameAsync()");
            proxy.ShouldContain("public virtual async Task DeleteAsync()");
            proxy.ShouldContain("public virtual IAsyncEnumerable<String> GetBooks()");
            proxy.ShouldContain("return RequestAsyncEnumerable<String>(nameof(GetBooks));");
            proxy.ShouldContain("public virtual IAsyncEnumerable<Int32> Read(CancellationToken cancellationToken, int count)");
            proxy.ShouldContain("GetCount()");
            proxy.Split("NotImplementedException").Length.ShouldBe(2);

            var interfacePath = Path.Combine(folder, "IStreamingAppService.cs");
            if (withoutContracts)
            {
                File.Exists(interfacePath).ShouldBeFalse();
                return;
            }

            var @interface = await File.ReadAllTextAsync(interfacePath);
            @interface.ShouldContain("IAsyncEnumerable<Int32> GetItemsAsync();");
            @interface.ShouldContain("IAsyncEnumerable<String> GetNamedItemsAsync(string name);");
            @interface.ShouldNotContain("Task<IAsyncEnumerable");
            @interface.ShouldContain("Task<String> GetNameAsync();");
            @interface.ShouldContain("Task DeleteAsync();");
            @interface.ShouldContain("IAsyncEnumerable<String> GetBooks();");
            @interface.ShouldContain("IAsyncEnumerable<Int32> Read(CancellationToken cancellationToken, int count);");
            @interface.ShouldContain(" GetCount();");
            @interface.ShouldNotContain("Task<Int32> GetCount");
        }
        finally
        {
            Directory.Delete(workDirectory, true);
        }
    }

    private class TestCSharpServiceProxyGenerator : CSharpServiceProxyGenerator
    {
        public TestCSharpServiceProxyGenerator(CliHttpClientFactory cliHttpClientFactory, IJsonSerializer jsonSerializer)
            : base(cliHttpClientFactory, jsonSerializer)
        {
        }

        protected override Task<ApplicationApiDescriptionModel> GetApplicationApiDescriptionModelAsync(GenerateProxyArgs args, ApplicationApiDescriptionModelRequestDto requestDto = null)
        {
            return Task.FromResult(JsonSerializer.Deserialize<ApplicationApiDescriptionModel>(ApiDefinition));
        }
    }

    private const string ApiDefinition = """
    {
      "modules": {
        "app": {
          "rootPath": "app",
          "remoteServiceName": "Default",
          "controllers": {
            "MyProject.Streaming.StreamingAppService": {
              "controllerName": "Streaming",
              "type": "MyProject.Streaming.StreamingAppService",
              "interfaces": [
                {
                  "type": "MyProject.Streaming.IStreamingAppService",
                  "name": "IStreamingAppService",
                  "methods": [
                    {
                      "name": "GetItemsAsync",
                      "parametersOnMethod": [],
                      "returnValue": { "type": "System.Collections.Generic.IAsyncEnumerable<System.Int32>", "typeSimple": "System.Collections.Generic.IAsyncEnumerable<number>" }
                    },
                    {
                      "name": "GetNamedItemsAsync",
                      "parametersOnMethod": [ { "name": "name", "type": "System.String", "typeSimple": "string" } ],
                      "returnValue": { "type": "System.Collections.Generic.IAsyncEnumerable<System.String>", "typeSimple": "System.Collections.Generic.IAsyncEnumerable<string>" }
                    },
                    {
                      "name": "GetNameAsync",
                      "parametersOnMethod": [],
                      "returnValue": { "type": "System.String", "typeSimple": "string" }
                    },
                    {
                      "name": "DeleteAsync",
                      "parametersOnMethod": [],
                      "returnValue": { "type": "System.Void", "typeSimple": "System.Void" }
                    },
                    {
                      "name": "GetBooks",
                      "parametersOnMethod": [],
                      "returnValue": { "type": "System.Collections.Generic.IAsyncEnumerable<System.String>", "typeSimple": "System.Collections.Generic.IAsyncEnumerable<string>" }
                    },
                    {
                      "name": "Read",
                      "parametersOnMethod": [
                        { "name": "cancellationToken", "type": "System.Threading.CancellationToken", "typeSimple": "System.Threading.CancellationToken" },
                        { "name": "count", "type": "System.Int32", "typeSimple": "number" }
                      ],
                      "returnValue": { "type": "System.Collections.Generic.IAsyncEnumerable<System.Int32>", "typeSimple": "System.Collections.Generic.IAsyncEnumerable<number>" }
                    },
                    {
                      "name": "GetCount",
                      "parametersOnMethod": [],
                      "returnValue": { "type": "System.Int32", "typeSimple": "number" }
                    }
                  ]
                }
              ],
              "actions": {
                "GetItemsAsync": {
                  "uniqueName": "GetItemsAsync",
                  "name": "GetItemsAsync",
                  "httpMethod": "GET",
                  "url": "api/app/streaming/items",
                  "parametersOnMethod": [],
                  "parameters": [],
                  "returnValue": { "type": "System.Collections.Generic.IAsyncEnumerable<System.Int32>", "typeSimple": "System.Collections.Generic.IAsyncEnumerable<number>" },
                  "implementFrom": "MyProject.Streaming.IStreamingAppService"
                },
                "GetNamedItemsAsyncByName": {
                  "uniqueName": "GetNamedItemsAsyncByName",
                  "name": "GetNamedItemsAsync",
                  "httpMethod": "GET",
                  "url": "api/app/streaming/named-items",
                  "parametersOnMethod": [ { "name": "name", "type": "System.String", "typeSimple": "string" } ],
                  "parameters": [ { "nameOnMethod": "name", "name": "name", "type": "System.String", "typeSimple": "string" } ],
                  "returnValue": { "type": "System.Collections.Generic.IAsyncEnumerable<System.String>", "typeSimple": "System.Collections.Generic.IAsyncEnumerable<string>" },
                  "implementFrom": "MyProject.Streaming.IStreamingAppService"
                },
                "GetNameAsync": {
                  "uniqueName": "GetNameAsync",
                  "name": "GetNameAsync",
                  "httpMethod": "GET",
                  "url": "api/app/streaming/name",
                  "parametersOnMethod": [],
                  "parameters": [],
                  "returnValue": { "type": "System.String", "typeSimple": "string" },
                  "implementFrom": "MyProject.Streaming.IStreamingAppService"
                },
                "DeleteAsync": {
                  "uniqueName": "DeleteAsync",
                  "name": "DeleteAsync",
                  "httpMethod": "DELETE",
                  "url": "api/app/streaming",
                  "parametersOnMethod": [],
                  "parameters": [],
                  "returnValue": { "type": "System.Void", "typeSimple": "System.Void" },
                  "implementFrom": "MyProject.Streaming.IStreamingAppService"
                },
                "GetBooks": {
                  "uniqueName": "GetBooks",
                  "name": "GetBooks",
                  "httpMethod": "GET",
                  "url": "api/app/streaming/books",
                  "parametersOnMethod": [],
                  "parameters": [],
                  "returnValue": { "type": "System.Collections.Generic.IAsyncEnumerable<System.String>", "typeSimple": "System.Collections.Generic.IAsyncEnumerable<string>" },
                  "implementFrom": "MyProject.Streaming.IStreamingAppService"
                },
                "GetCount": {
                  "uniqueName": "GetCount",
                  "name": "GetCount",
                  "httpMethod": "GET",
                  "url": "api/app/streaming/count",
                  "parametersOnMethod": [],
                  "parameters": [],
                  "returnValue": { "type": "System.Int32", "typeSimple": "number" },
                  "implementFrom": "MyProject.Streaming.IStreamingAppService"
                },
                "ReadByCancellationTokenAndCount": {
                  "uniqueName": "ReadByCancellationTokenAndCount",
                  "name": "Read",
                  "httpMethod": "GET",
                  "url": "api/app/streaming/read",
                  "parametersOnMethod": [
                    { "name": "cancellationToken", "type": "System.Threading.CancellationToken", "typeSimple": "System.Threading.CancellationToken" },
                    { "name": "count", "type": "System.Int32", "typeSimple": "number" }
                  ],
                  "parameters": [ { "nameOnMethod": "count", "name": "count", "type": "System.Int32", "typeSimple": "number" } ],
                  "returnValue": { "type": "System.Collections.Generic.IAsyncEnumerable<System.Int32>", "typeSimple": "System.Collections.Generic.IAsyncEnumerable<number>" },
                  "implementFrom": "MyProject.Streaming.IStreamingAppService"
                }
              }
            }
          }
        }
      },
      "types": {}
    }
    """;
}
