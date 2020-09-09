using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Modularity;
using Volo.Docs.Projects;
using Xunit;

namespace Volo.Docs
{
    public abstract class ProjectRepository_Tests<TStartupModule> : DocsTestBase<TStartupModule>
        where TStartupModule : IAbpModule
    {
        protected readonly IProjectRepository ProjectRepository;

        protected ProjectRepository_Tests()
        {
            ProjectRepository = GetRequiredService<IProjectRepository>(); ;
        }

        [Fact]
        public async Task GetListAsync()
        {
            var projects = await ProjectRepository.GetListAsync();

            projects.Count.ShouldBe(1);
        }
        
        [Fact]
        public async Task GetByShortNameAsync()
        {
            var project = await ProjectRepository.GetByShortNameAsync("abp");

            project.ShouldNotBeNull();
            project.ShortName.ShouldBe("abp");
        }
    }
}
