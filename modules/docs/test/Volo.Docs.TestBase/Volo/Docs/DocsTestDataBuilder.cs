using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Docs.GitHub.Documents;
using Volo.Docs.Projects;

namespace Volo.Docs
{
    public class DocsTestDataBuilder : ITransientDependency
    {
        private readonly IGuidGenerator _guidGenerator;
        private DocsTestData _testData;
        private IProjectRepository _projectRepository;

        public DocsTestDataBuilder(
            IGuidGenerator guidGenerator,
            DocsTestData testData, IProjectRepository projectRepository)
        {
            _guidGenerator = guidGenerator;
            _testData = testData;
            _projectRepository = projectRepository;
        }

        public void Build()
        {
            var project = new Project(_testData.PorjectId, "ABP vNext", "ABP", GithubDocumentStore.Type, "md", "index",
                "docs-nav.json");
            project.ExtraProperties.Add("GitHubRootUrl", "https://github.com/abpframework/abp/tree/{version}/docs/en/");
            project.ExtraProperties.Add("GitHubAccessToken", "123456");
            project.ExtraProperties.Add("GitHubUserAgent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64)");

            _projectRepository.Insert(project);
        }
    }
}