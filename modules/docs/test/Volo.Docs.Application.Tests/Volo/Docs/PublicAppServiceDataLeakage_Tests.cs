using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using NSubstitute;
using Shouldly;
using Volo.Abp.Content;
using Volo.Abp.Application.Services;
using Volo.Abp.Data;
using Volo.Abp.Uow;
using Volo.Docs.Common.Documents;
using Volo.Docs.Common.Projects;
using Volo.Docs.Documents;
using Volo.Docs.Documents.FullSearch.Elastic;
using Volo.Docs.GitHub.Documents;
using Volo.Docs.Projects;
using Volo.Docs.Projects.Pdf;
using Xunit;

namespace Volo.Docs
{
    public class PublicAppServiceDataLeakage_Tests : DocsApplicationTestBase
    {
        private static readonly string[] SecretNames =
        {
            "GitHubAccessToken", "GitHubUserAgent", "CustomSecret", "RegisteredSecret", "Path", "NestedSecret"
        };

        protected override void AfterAddApplication(IServiceCollection services)
        {
            base.AfterAddApplication(services);
            var environment = Substitute.For<IHostEnvironment>();
            environment.EnvironmentName.Returns(Environments.Production);
            services.AddSingleton(environment);

            var search = Substitute.For<IDocumentFullSearch>();
            search.SearchAsync(default, default, default, default).ReturnsForAnyArgs(new EsDocumentResult
            {
                TotalCount = 1,
                EsDocuments = new List<EsDocument>
                {
                    new EsDocument { Name = "CLI.md", FileName = "CLI.md", Version = "2.0.0", LanguageCode = "en", Highlight = new List<string> { "Public snippet" } }
                }
            });
            services.AddSingleton(search);

            var fileStore = Substitute.For<IProjectPdfFileStore>();
            fileStore.GetOrNullAsync(default, default, default)
                .ReturnsForAnyArgs(_ => new MemoryStream(Encoding.UTF8.GetBytes("Public document")));
            services.AddSingleton(fileStore);
            services.Configure<DocsElasticSearchOptions>(options => options.Enable = false);
        }

        public static IEnumerable<object[]> PublicMethods()
        {
            foreach (var service in PublicContracts())
            {
                foreach (var method in service.GetMethods())
                {
                    yield return new object[] { service.Name + "." + method.Name };
                }
            }
        }

        private static IEnumerable<Type> PublicContracts()
        {
            return new[] { typeof(IProjectAppService).Assembly, typeof(IDocumentAppService).Assembly }
                .SelectMany(assembly => assembly.GetExportedTypes())
                .Where(type => type.IsInterface && typeof(IApplicationService).IsAssignableFrom(type));
        }

        [Fact]
        public void Should_Cover_All_Public_Application_Service_Methods()
        {
            var contracts = PublicContracts().ToArray();
            foreach (var contract in contracts)
            {
                contract.GetMethods().Select(method => method.Name).Distinct().Count()
                    .ShouldBe(contract.GetMethods().Length, "Overloaded methods need signature-specific disclosure tests.");
            }

            foreach (var implementation in new[] { typeof(DocumentAppService).Assembly, typeof(ProjectAppService).Assembly }
                .SelectMany(assembly => assembly.GetExportedTypes())
                .Where(type => type.IsClass && !type.IsAbstract && typeof(IApplicationService).IsAssignableFrom(type)))
            {
                var methods = implementation.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                    .Where(method => !method.IsSpecialName).Select(MethodSignature);
                var covered = contracts.Where(contract => contract.IsAssignableFrom(implementation))
                    .SelectMany(contract => contract.GetMethods()).Select(MethodSignature).Distinct();
                methods.ShouldBe(covered, ignoreOrder: true);
            }
        }

        private static string MethodSignature(MethodInfo method)
        {
            return method.Name + "(" + string.Join(",", method.GetParameters().Select(parameter => parameter.ParameterType.FullName)) + ")";
        }

        [Theory]
        [MemberData(nameof(PublicMethods))]
        public async Task Should_Not_Expose_Project_Secrets_From_Any_Public_Method(string method)
        {
            var testData = GetRequiredService<DocsTestData>();
            var repository = GetRequiredService<IProjectRepository>();
            var github = GetRequiredService<IGithubRepositoryManager>();
            github.GetFileRawStringContentAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>())
                .Returns(call =>
                {
                    var url = call.ArgAt<string>(0);
                    if (url.Contains("docs-nav.json", StringComparison.Ordinal))
                    {
                        return "{\"items\":[{\"text\":\"CLI\",\"path\":\"CLI.md\",\"items\":[]}]}";
                    }
                    if (url.Contains("docs-params.json", StringComparison.Ordinal))
                    {
                        return "{\"parameters\":[{\"name\":\"platform\",\"displayName\":\"Platform\",\"values\":{\"dotnet\":\"Dotnet\"}}]}";
                    }
                    if (url.Contains(DocsDomainConsts.LanguageConfigFileName, StringComparison.Ordinal))
                    {
                        return "{\"languages\":[{\"code\":\"en\",\"displayName\":\"English\",\"isDefault\":true}]}";
                    }
                    return "Public document";
                });

            using (var uow = GetRequiredService<IUnitOfWorkManager>().Begin())
            {
                var project = await repository.GetAsync(testData.ProjectId);
                foreach (var name in SecretNames)
                {
                    project.SetProperty(name, "PRIVATE_MARKER_" + name);
                }
                project.SetProperty("NestedSecret", new Dictionary<string, object> { ["value"] = "PRIVATE_MARKER_NestedSecret" });
                project.SetProperty("GithubVersionProviderSource", GithubVersionProviderSource.Releases);
                project.SetProperty("VersionBranchPrefix", "rel-");
                project.SetDefaultDocumentName("CLI");
                project.AddPdfFile(project.Id, "abp-2.0.0-en.zip", "2.0.0", "en");
                await repository.UpdateAsync(project, autoSave: true);
                var original = JsonSerializer.Serialize(project.ExtraProperties);

                var result = await InvokeAsync(method, project.Id);
                result.ShouldNotBeNull();
                if (result is IRemoteStreamContent remote)
                {
                    using (remote)
                    {
                        remote.FileName.ShouldBe("abp-2.0.0-en.zip");
                        using var reader = new StreamReader(remote.GetStream());
                        var content = await reader.ReadToEndAsync();
                        content.ShouldBe("Public document");
                        result = new { remote.FileName, remote.ContentType, Content = content };
                    }
                }
                var json = JsonSerializer.Serialize(result, result.GetType(), new JsonSerializerOptions(JsonSerializerDefaults.Web));
                json.ShouldNotContain("PRIVATE_MARKER_");
                using var document = JsonDocument.Parse(json);
                AssertNoPrivateProperties(document.RootElement);
                JsonSerializer.Serialize(project.ExtraProperties).ShouldBe(original);
                await uow.CompleteAsync();
            }

            var saved = await repository.GetAsync(testData.ProjectId);
            foreach (var name in SecretNames.Where(name => name != "NestedSecret"))
            {
                saved.GetProperty<string>(name).ShouldBe("PRIVATE_MARKER_" + name);
            }
        }

        private async Task<object> InvokeAsync(string method, Guid projectId)
        {
            var projects = GetRequiredService<IProjectAppService>();
            var documents = GetRequiredService<IDocumentAppService>();
            switch (method)
            {
                case "IProjectAppService.GetListAsync":
                    var list = await projects.GetListAsync();
                    list.Items.ShouldHaveSingleItem();
                    return list;
                case "IProjectAppService.GetAsync":
                    return await projects.GetAsync("abp");
                case "IProjectAppService.GetVersionsAsync":
                    var versions = await projects.GetVersionsAsync("abp");
                    versions.Items.ShouldHaveSingleItem();
                    return versions;
                case "IProjectAppService.GetDefaultLanguageCodeAsync":
                    var language = await projects.GetDefaultLanguageCodeAsync("abp", "2.0.0");
                    language.ShouldBe("en");
                    return language;
                case "IProjectAppService.GetLanguageListAsync":
                    var languages = await projects.GetLanguageListAsync("abp", "2.0.0");
                    languages.Languages.ShouldHaveSingleItem().Code.ShouldBe("en");
                    return languages;
                case "IDocumentAppService.GetAsync":
                    return await documents.GetAsync(new GetDocumentInput { ProjectId = projectId, Name = "CLI.md", LanguageCode = "en", Version = "2.0.0" });
                case "IDocumentAppService.GetDefaultAsync":
                    return await documents.GetDefaultAsync(new GetDefaultDocumentInput { ProjectId = projectId, LanguageCode = "en", Version = "2.0.0" });
                case "IDocumentAppService.GetNavigationAsync":
                    var navigation = await documents.GetNavigationAsync(new GetNavigationDocumentInput { ProjectId = projectId, LanguageCode = "en", Version = "2.0.0" });
                    navigation.Items.ShouldHaveSingleItem().Path.ShouldBe("CLI.md");
                    return navigation;
                case "IDocumentAppService.GetParametersAsync":
                    var parameters = await documents.GetParametersAsync(new GetParametersDocumentInput { ProjectId = projectId, LanguageCode = "en", Version = "2.0.0" });
                    parameters.Parameters.ShouldHaveSingleItem().Name.ShouldBe("platform");
                    return parameters;
                case "IDocumentAppService.GetResourceAsync":
                    var resource = await documents.GetResourceAsync(new GetDocumentResourceInput { ProjectId = projectId, Name = "image.png", LanguageCode = "en", Version = "2.0.0" });
                    resource.Content.ShouldBe(new byte[] { 1, 2, 3 });
                    return resource;
                case "IDocumentAppService.SearchAsync":
                    var search = await documents.SearchAsync(new DocumentSearchInput { ProjectId = projectId, Context = "CLI", LanguageCode = "en", Version = "2.0.0" });
                    search.TotalCount.ShouldBe(1);
                    search.Items.ShouldHaveSingleItem().Name.ShouldBe("CLI.md");
                    return search;
                case "IDocumentAppService.FullSearchEnabledAsync":
                    var enabled = await documents.FullSearchEnabledAsync();
                    enabled.ShouldBeFalse();
                    return enabled;
                case "IDocumentAppService.GetUrlsAsync":
                    var urls = await documents.GetUrlsAsync("/docs/");
                    urls.ShouldNotBeEmpty();
                    urls.ShouldContain("/docs/en/abp/2.0.0/CLI");
                    return urls;
                case "IDocumentPdfAppService.DownloadPdfAsync":
                    return await GetRequiredService<IDocumentPdfAppService>().DownloadPdfAsync(new DocumentPdfGeneratorInput { ProjectId = projectId, LanguageCode = "en", Version = "2.0.0" });
                case "IDocumentPdfAppService.ExistsAsync":
                    var exists = await GetRequiredService<IDocumentPdfAppService>().ExistsAsync(new DocumentPdfGeneratorInput { ProjectId = projectId, LanguageCode = "en", Version = "2.0.0" });
                    exists.ShouldBeTrue();
                    return exists;
                default:
                    throw new InvalidOperationException("Add a disclosure regression case for " + method);
            }
        }

        private static void AssertNoPrivateProperties(JsonElement element)
        {
            if (element.ValueKind == JsonValueKind.Object)
            {
                foreach (var property in element.EnumerateObject())
                {
                    SecretNames.ShouldNotContain(property.Name);
                    if (property.Name == "extraProperties")
                    {
                        property.Value.EnumerateObject().Select(item => item.Name)
                            .ShouldBe(new[] { "GithubVersionProviderSource", "VersionBranchPrefix" }, ignoreOrder: true);
                    }
                    AssertNoPrivateProperties(property.Value);
                }
            }
            else if (element.ValueKind == JsonValueKind.Array)
            {
                foreach (var item in element.EnumerateArray())
                {
                    AssertNoPrivateProperties(item);
                }
            }
        }
    }
}
