using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Shouldly;
using Volo.Docs.Admin.Projects;
using Volo.Docs.Projects;
using Xunit;

namespace Volo.Docs
{
    public class ApplicationService_Tests : DocsApplicationTestBase
    {
        private readonly IProjectAppService _projectAppService;
        private readonly IProjectRepository _projectRepository;
        private readonly DocsTestData _testData;

        public ApplicationService_Tests()
        {
            _projectRepository = GetRequiredService<IProjectRepository>();
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
            var project = await _projectAppService.GetAsync("ABP");
            project.ShouldNotBeNull();
            project.ShortName.ShouldBe("ABP");
        }

        [Fact]
        public async Task GetVersionsAsync()
        {
            // TODO: Need to mock WebClient and Octokit components
        }

    }

}
