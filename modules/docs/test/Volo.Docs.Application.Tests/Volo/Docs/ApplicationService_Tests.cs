using System;
using System.Threading.Tasks;
using NSubstitute;
using Shouldly;
using Volo.Docs.Common.Projects;
using Volo.Docs.Documents;
using Volo.Docs.GitHub.Documents;
using Xunit;

namespace Volo.Docs
{
    public class ApplicationService_Tests : DocsApplicationTestBase
    {
        private readonly IDocumentAppService _documentAppService;
        private readonly IDocumentRepository _documentRepository;
        private readonly IGithubRepositoryManager _githubRepositoryManager;
        private readonly IProjectAppService _projectAppService;
        private readonly DocsTestData _testData;

        public ApplicationService_Tests()
        {
            _documentAppService = GetRequiredService<IDocumentAppService>();
            _documentRepository = GetRequiredService<IDocumentRepository>();
            _githubRepositoryManager = GetRequiredService<IGithubRepositoryManager>();
            _projectAppService = GetRequiredService<IProjectAppService>();
            _testData = GetRequiredService<DocsTestData>();
        }

        [Fact]
        public async Task GetListAsync()
        {
            var projects = await _projectAppService.GetListAsync();
            projects.ShouldNotBeNull();
            projects.Items.Count.ShouldBe(1);
            projects.Items.ShouldContain(x => x.Id == _testData.ProjectId);
        }

        [Fact]
        public async Task GetAsync()
        {
            var project = await _projectAppService.GetAsync("abp");
            project.ShouldNotBeNull();
            project.ShortName.ShouldBe("abp");
        }

        [Fact]
        public async Task GetVersionsAsync()
        {
            var versions = await _projectAppService.GetVersionsAsync("ABP");
            versions.ShouldNotBeNull();
            versions.Items.Count.ShouldBe(1);
            versions.Items.ShouldContain(x => x.Name == "0.15.0" && x.DisplayName == "0.15.0");
        }

        [Fact]
        public async Task GetAsync_Should_Not_Set_Stale_Cache_Fallback_Flag_For_Fresh_Cache()
        {
            var document = await _documentAppService.GetAsync(CreateDocumentInput());

            document.IsStaleCacheFallback.ShouldBeFalse();
            document.Content.ShouldBe("this is abp cli");
        }

        [Fact]
        public async Task GetAsync_Should_Set_Stale_Cache_Fallback_Flag_When_Refresh_Fails()
        {
            await MakeDocumentStaleAsync();
            ConfigureRefreshFailure();

            var document = await _documentAppService.GetAsync(CreateDocumentInput());

            document.IsStaleCacheFallback.ShouldBeTrue();
            document.Content.ShouldBe("this is abp cli");
        }

        [Fact]
        public async Task GetAsync_Should_Not_Set_Stale_Cache_Fallback_Flag_When_Refresh_Succeeds()
        {
            await MakeDocumentStaleAsync();

            var document = await _documentAppService.GetAsync(CreateDocumentInput());

            document.IsStaleCacheFallback.ShouldBeFalse();
            document.Content.ShouldBe("stringContent");
        }

        private GetDocumentInput CreateDocumentInput()
        {
            return new GetDocumentInput
            {
                ProjectId = _testData.ProjectId,
                Name = "CLI.md",
                LanguageCode = "en",
                Version = "2.0.0"
            };
        }

        private async Task MakeDocumentStaleAsync()
        {
            await _documentRepository.UpdateProjectLastCachedTimeAsync(_testData.ProjectId, DateTime.MinValue);
        }

        private void ConfigureRefreshFailure()
        {
            _githubRepositoryManager.GetFileRawStringContentAsync(
                    Arg.Is<string>(url =>
                        url.EndsWith("/en/CLI.md", StringComparison.OrdinalIgnoreCase) ||
                        url.EndsWith("/en/CLI/index.md", StringComparison.OrdinalIgnoreCase) ||
                        url.EndsWith("/en/CLI/Index.md", StringComparison.OrdinalIgnoreCase)),
                    Arg.Any<string>(),
                    Arg.Any<string>())
                .Returns(_ => Task.FromException<string>(new Exception("Simulated refresh failure")));
        }
    }
}
