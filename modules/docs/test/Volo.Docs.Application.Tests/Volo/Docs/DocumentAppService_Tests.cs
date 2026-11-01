using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NSubstitute;
using Shouldly;
using Volo.Abp.Data;
using Volo.Abp.Uow;
using Volo.Docs.Documents;
using Volo.Docs.GitHub.Documents;
using Volo.Docs.Projects;
using Xunit;

namespace Volo.Docs
{
    public class DocumentAppService_Tests : DocsApplicationTestBase
    {
        protected override void AfterAddApplication(IServiceCollection services)
        {
            base.AfterAddApplication(services);

            var environment = Substitute.For<IHostEnvironment>();
            environment.EnvironmentName.Returns(Environments.Production);
            services.AddSingleton(environment);
        }

        [Theory]
        [InlineData(false, "Cached")]
        [InlineData(true, "Cached")]
        [InlineData(false, "Fresh")]
        [InlineData(true, "Fresh")]
        [InlineData(false, "ExpiredRefresh")]
        [InlineData(true, "ExpiredRefresh")]
        [InlineData(false, "ExpiredFallback")]
        [InlineData(true, "ExpiredFallback")]
        [InlineData(false, "Development")]
        [InlineData(true, "Development")]
        public async Task Should_Not_Expose_Private_Project_Properties(bool defaultDocument, string retrievalMode)
        {
            const string token = "LOCAL_TEST_TOKEN_MARKER";
            const string userAgent = "LOCAL_TEST_USER_AGENT_MARKER";
            const string customSecret = "LOCAL_TEST_CUSTOM_SECRET_MARKER";
            var testData = GetRequiredService<DocsTestData>();
            var projectRepository = GetRequiredService<IProjectRepository>();
            var documentAppService = GetRequiredService<IDocumentAppService>();
            var repositoryManager = GetRequiredService<IGithubRepositoryManager>();
            var documentName = retrievalMode == "Fresh" ? "Uncached.md" : "CLI.md";

            if (retrievalMode == "Development")
            {
                GetRequiredService<IHostEnvironment>().EnvironmentName = Environments.Development;
            }

            using (var uow = GetRequiredService<IUnitOfWorkManager>().Begin())
            {
                var project = await projectRepository.GetAsync(testData.ProjectId);
                project.SetProperty("GitHubAccessToken", token);
                project.SetProperty("GitHubUserAgent", userAgent);
                project.SetProperty("CustomSecret", customSecret);
                project.SetProperty("GithubVersionProviderSource", GithubVersionProviderSource.Branches);
                project.SetProperty("VersionBranchPrefix", "2.");
                project.SetDefaultDocumentName(retrievalMode == "Fresh" ? "Uncached" : "CLI");
                await projectRepository.UpdateAsync(project, autoSave: true);

                if (retrievalMode is "ExpiredFallback" or "ExpiredRefresh")
                {
                    var documentRepository = GetRequiredService<IDocumentRepository>();
                    var cachedDocument = await documentRepository.FindAsync(project.Id, documentName, "en", "2.0.0");
                    cachedDocument.LastCachedTime = DateTime.Now.AddDays(-1);
                    await documentRepository.UpdateAsync(cachedDocument, autoSave: true);
                    if (retrievalMode == "ExpiredFallback")
                    {
                        repositoryManager.GetFileRawStringContentAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>())
                            .Returns(Task.FromException<string>(new HttpRequestException("Document source unavailable.")));
                    }
                }

                var document = defaultDocument
                    ? await documentAppService.GetDefaultAsync(new GetDefaultDocumentInput
                    {
                        ProjectId = project.Id,
                        LanguageCode = "en",
                        Version = "0.0"
                    })
                    : await documentAppService.GetAsync(new GetDocumentInput
                    {
                        ProjectId = project.Id,
                        Name = documentName,
                        LanguageCode = "en",
                        Version = "0.0"
                    });

                document.Name.ShouldBe(documentName);
                document.Version.ShouldBe("2.0.0");
                document.Content.ShouldBe(retrievalMode is "Fresh" or "Development" or "ExpiredRefresh" ? "stringContent" : "this is abp cli");
                document.Project.Id.ShouldBe(project.Id);
                document.Project.GetProperty<string>("GitHubAccessToken").ShouldBeNull();
                document.Project.GetProperty<string>("GitHubUserAgent").ShouldBeNull();
                document.Project.ExtraProperties.Keys.ShouldBe(new[] { "GithubVersionProviderSource", "VersionBranchPrefix" }, ignoreOrder: true);
                document.Project.GetProperty<GithubVersionProviderSource>("GithubVersionProviderSource")
                    .ShouldBe(GithubVersionProviderSource.Branches);
                document.Project.GetProperty<string>("VersionBranchPrefix").ShouldBe("2.");

                var json = JsonSerializer.Serialize(document, new JsonSerializerOptions(JsonSerializerDefaults.Web));
                json.ShouldNotContain(token);
                json.ShouldNotContain(userAgent);
                json.ShouldNotContain(customSecret);
                project.GetProperty<string>("GitHubAccessToken").ShouldBe(token);
                project.GetProperty<string>("GitHubUserAgent").ShouldBe(userAgent);
                project.GetProperty<string>("CustomSecret").ShouldBe(customSecret);
                project.GetProperty<string>("GitHubRootUrl").ShouldNotBeNullOrEmpty();

                if (retrievalMode == "Cached")
                {
                    await repositoryManager.DidNotReceive().GetFileRawStringContentAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>());
                }
                else
                {
                    await repositoryManager.Received().GetFileRawStringContentAsync(Arg.Any<string>(), token, userAgent);
                }

                await uow.CompleteAsync();
            }

            var savedProject = await projectRepository.GetAsync(testData.ProjectId);
            savedProject.GetProperty<string>("GitHubAccessToken").ShouldBe(token);
            savedProject.GetProperty<string>("GitHubUserAgent").ShouldBe(userAgent);
            savedProject.GetProperty<string>("CustomSecret").ShouldBe(customSecret);
            savedProject.GetProperty<string>("GitHubRootUrl").ShouldNotBeNullOrEmpty();
        }
    }
}
