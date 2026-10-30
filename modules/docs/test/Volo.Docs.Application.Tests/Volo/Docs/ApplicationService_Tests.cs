using System.Text.Json;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Data;
using Volo.Abp.Uow;
using Volo.Docs.Common.Projects;
using Volo.Docs.Projects;
using Xunit;

namespace Volo.Docs
{
    public class ApplicationService_Tests : DocsApplicationTestBase
    {
        private readonly IProjectAppService _projectAppService;
        private readonly DocsTestData _testData;

        public ApplicationService_Tests()
        {
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
            projects.Items[0].ExtraProperties.ShouldBeEmpty();
        }

        [Fact]
        public async Task GetAsync()
        {
            var project = await _projectAppService.GetAsync("abp");
            project.ShouldNotBeNull();
            project.ShortName.ShouldBe("abp");
            project.ExtraProperties.ShouldBeEmpty();
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async Task Should_Only_Return_Public_Project_Properties(bool list)
        {
            const string secret = "LOCAL_TEST_UNKNOWN_SECRET_MARKER";
            var repository = GetRequiredService<IProjectRepository>();

            using (var uow = GetRequiredService<IUnitOfWorkManager>().Begin())
            {
                var project = await repository.GetAsync(_testData.ProjectId);
                project.SetProperty("CustomSecret", secret);
                project.SetProperty("GithubVersionProviderSource", GithubVersionProviderSource.Branches);
                project.SetProperty("VersionBranchPrefix", "rel-");
                await repository.UpdateAsync(project, autoSave: true);

                var result = list
                    ? (await _projectAppService.GetListAsync()).Items[0]
                    : await _projectAppService.GetAsync("abp");

                result.ExtraProperties.Keys.ShouldBe(new[] { "GithubVersionProviderSource", "VersionBranchPrefix" }, ignoreOrder: true);
                result.GetProperty<GithubVersionProviderSource>("GithubVersionProviderSource")
                    .ShouldBe(GithubVersionProviderSource.Branches);
                result.GetProperty<string>("VersionBranchPrefix").ShouldBe("rel-");
                JsonSerializer.Serialize(result).ShouldNotContain(secret);
                project.GetProperty<string>("CustomSecret").ShouldBe(secret);
                project.GetProperty<string>("GitHubAccessToken").ShouldBe("123456");
                project.GetProperty<string>("GitHubRootUrl").ShouldNotBeNullOrEmpty();

                await uow.CompleteAsync();
            }

            var savedProject = await repository.GetAsync(_testData.ProjectId);
            savedProject.GetProperty<string>("CustomSecret").ShouldBe(secret);
            savedProject.GetProperty<string>("GitHubAccessToken").ShouldBe("123456");
        }

        [Fact]
        public async Task GetVersionsAsync()
        {
            var versions = await _projectAppService.GetVersionsAsync("ABP");
            versions.ShouldNotBeNull();
            versions.Items.Count.ShouldBe(1);
            versions.Items.ShouldContain(x => x.Name == "0.15.0" && x.DisplayName == "0.15.0");
        }
    }
}
