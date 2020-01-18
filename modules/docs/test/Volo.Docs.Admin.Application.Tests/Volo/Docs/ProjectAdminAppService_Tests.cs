using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Application.Dtos;
using Volo.Docs.Admin.Projects;
using Volo.Docs.Projects;
using Xunit;

namespace Volo.Docs
{
    public class ProjectAdminAppService_Tests : DocsAdminApplicationTestBase
    {
        private readonly IProjectAdminAppService _projectAdminAppService;
        private readonly IProjectRepository _projectRepository;
        private readonly DocsTestData _testData;

        public ProjectAdminAppService_Tests()
        {
            _projectRepository = GetRequiredService<IProjectRepository>();
            _projectAdminAppService = GetRequiredService<IProjectAdminAppService>();
            _testData = GetRequiredService<DocsTestData>();
        }

        [Fact]
        public async Task GetListAsync()
        {
            var projects = await _projectAdminAppService.GetListAsync(new PagedAndSortedResultRequestDto());
            projects.ShouldNotBeNull();
            projects.TotalCount.ShouldBe(1);
            projects.Items.ShouldContain(x => x.Name == "ABP vNext");
        }

        [Fact]
        public async Task GetAsync()
        {
            var project = await _projectAdminAppService.GetAsync(_testData.PorjectId);
            project.ShouldNotBeNull();
            project.Id.ShouldBe(_testData.PorjectId);
        }

        [Fact]
        public async Task CreateAsync()
        {
            var createProjectDto = new CreateProjectDto
            {
                Name = "ABP vNext",
                ShortName = "ABP",
                Format = "md",
                DefaultDocumentName = "index",
                NavigationDocumentName = "docs-nav.json",
                ParametersDocumentName = "docs-params.json",
                MinimumVersion = "1",
                MainWebsiteUrl = "abp.io",
                LatestVersionBranchName = "",
                DocumentStoreType = "GitHub",
                ExtraProperties = new Dictionary<string, object>()
            };
            createProjectDto.ExtraProperties.Add("GitHubRootUrl",
                "https://github.com/abpframework/abp/tree/{version}/docs/en/");
            createProjectDto.ExtraProperties.Add("GitHubAccessToken", "123456");
            createProjectDto.ExtraProperties.Add("GitHubUserAgent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64)");

            //Act
            var projectDto = await _projectAdminAppService.CreateAsync(createProjectDto);

            //Assert
            projectDto.ShouldNotBeNull();
            projectDto.Name.ShouldBe(createProjectDto.Name);
            projectDto.ShortName.ShouldBe(createProjectDto.ShortName);
            projectDto.Format.ShouldBe(createProjectDto.Format);
            projectDto.DefaultDocumentName.ShouldBe(createProjectDto.DefaultDocumentName);
            projectDto.NavigationDocumentName.ShouldBe(createProjectDto.NavigationDocumentName);
            projectDto.MinimumVersion.ShouldBe(createProjectDto.MinimumVersion);
            projectDto.MainWebsiteUrl.ShouldBe(createProjectDto.MainWebsiteUrl);
            projectDto.LatestVersionBranchName.ShouldBe(createProjectDto.LatestVersionBranchName);
            projectDto.DocumentStoreType.ShouldBe(createProjectDto.DocumentStoreType);

            projectDto.ExtraProperties.Except(createProjectDto.ExtraProperties).Any().ShouldBe(false);
        }

        [Fact]
        public async Task UpdateAsync()
        {
            var updateProjectDto = new UpdateProjectDto
            {
                Name = "ABP vNext",
                Format = "md",
                DefaultDocumentName = "index",
                NavigationDocumentName = "docs-nav.json",

                MinimumVersion = "1",
                MainWebsiteUrl = "abp.io",
                LatestVersionBranchName = "",
                ExtraProperties = new Dictionary<string, object>()
            };
            updateProjectDto.ExtraProperties.Add("test", "test");

            var projectDto = await _projectAdminAppService.UpdateAsync(_testData.PorjectId, updateProjectDto);


            projectDto.ShouldNotBeNull();
            projectDto.Name.ShouldBe(updateProjectDto.Name);

            projectDto.Format.ShouldBe(updateProjectDto.Format);
            projectDto.DefaultDocumentName.ShouldBe(updateProjectDto.DefaultDocumentName);
            projectDto.NavigationDocumentName.ShouldBe(updateProjectDto.NavigationDocumentName);
            projectDto.MinimumVersion.ShouldBe(updateProjectDto.MinimumVersion);
            projectDto.MainWebsiteUrl.ShouldBe(updateProjectDto.MainWebsiteUrl);
            projectDto.LatestVersionBranchName.ShouldBe(updateProjectDto.LatestVersionBranchName);
            projectDto.ExtraProperties.ShouldContainKey("test");
        }

        [Fact]
        public async Task DeleteAsync()
        {
            (await _projectRepository.GetAsync(_testData.PorjectId)).ShouldNotBeNull();

            await _projectAdminAppService.DeleteAsync(_testData.PorjectId);

            (await _projectRepository.GetListAsync()).ShouldBeEmpty();
        }
    }
}
