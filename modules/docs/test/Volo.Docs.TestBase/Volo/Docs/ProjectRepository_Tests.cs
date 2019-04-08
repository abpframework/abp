using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Modularity;
using Volo.Docs.Projects;
using Xunit;

namespace Volo.Docs
{
    public abstract class ProjectRepository_Tests<TStartupModule> : DocsTestBase<TStartupModule>
        where TStartupModule : IAbpModule
    {

        protected readonly IProjectRepository _projectRepository;

        protected ProjectRepository_Tests()
        {
            _projectRepository = GetRequiredService<IProjectRepository>(); ;
        }

        [Fact]
        public async Task GetListAsync()
        {
            var projects = await _projectRepository.GetListAsync();

            projects.ShouldNotBeNull();
            projects.Count.ShouldBe(1);
        }

        [Fact]
        public async Task GetTotalProjectCount()
        {
            var count = await _projectRepository.GetTotalProjectCount();

            count.ShouldBe(1);
        }

        [Fact]
        public async Task GetByShortNameAsync()
        {
            var project = await _projectRepository.GetByShortNameAsync("ABP");

            project.ShouldNotBeNull();
            project.ShortName.ShouldBe("ABP");
        }
    }
}
