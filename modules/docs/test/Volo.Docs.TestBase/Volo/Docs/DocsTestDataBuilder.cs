using System;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Docs.Documents;
using Volo.Docs.GitHub.Documents;
using Volo.Docs.Projects;

namespace Volo.Docs
{
    public class DocsTestDataBuilder : ITransientDependency
    {
        private readonly DocsTestData _testData;
        private readonly IProjectRepository _projectRepository;
        private readonly IDocumentRepository _documentRepository;

        public DocsTestDataBuilder(
            DocsTestData testData, 
            IProjectRepository projectRepository, 
            IDocumentRepository documentRepository)
        {
            _testData = testData;
            _projectRepository = projectRepository;
            _documentRepository = documentRepository;
        }

        public async Task BuildAsync()
        {
            var project = new Project(
                _testData.PorjectId,
                "ABP vNext",
                "ABP",
                GithubDocumentSource.Type,
                "md",
                "index",
                "docs-nav.json",
                "docs-params.json"
            );

            project
                .SetProperty("GitHubRootUrl", "https://github.com/abpframework/abp/tree/{version}/docs/en/")
                .SetProperty("GitHubAccessToken", "123456")
                .SetProperty("GitHubUserAgent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64)");

            await _projectRepository.InsertAsync(project);

            await _documentRepository.InsertAsync(new Document(Guid.NewGuid(), project.Id, "CLI.md", "2.0.0", "en", "CLI.md",
                "this is abp cli", "md", "https://github.com/abpframework/abp/blob/2.0.0/docs/en/CLI.md",
                "https://github.com/abpframework/abp/tree/2.0.0/docs/",
                "https://raw.githubusercontent.com/abpframework/abp/2.0.0/docs/en/", "", DateTime.Now, DateTime.Now,
                DateTime.Now));
        }
    }
}