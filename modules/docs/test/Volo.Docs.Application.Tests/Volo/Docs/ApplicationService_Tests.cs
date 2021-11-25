using System.Threading.Tasks;
using Shouldly;
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
            projects.Items.ShouldContain(x => x.Id == _testData.PorjectId);
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
    }
}
