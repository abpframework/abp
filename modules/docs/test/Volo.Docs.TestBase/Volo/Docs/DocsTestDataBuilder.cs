using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Docs.GitHub.Documents;
using Volo.Docs.Projects;

namespace Volo.Docs
{
    public class DocsTestDataBuilder : ITransientDependency
    {
        private readonly DocsTestData _testData;
        private readonly IProjectRepository _projectRepository;

        public DocsTestDataBuilder(
            DocsTestData testData, 
            IProjectRepository projectRepository)
        {
            _testData = testData;
            _projectRepository = projectRepository;
        }

        public void Build()
        {
            var project = new Project(
                _testData.PorjectId,
                "ABP vNext",
                "ABP",
                GithubDocumentStore.Type,
                "md",
                "index",
                "docs-nav.json"
            );

            project
                .SetProperty("GitHubRootUrl", "https://github.com/abpframework/abp/tree/{version}/docs/en/")
                .SetProperty("GitHubAccessToken", "123456")
                .SetProperty("GitHubUserAgent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64)");

            _projectRepository.Insert(project);
        }
    }
}