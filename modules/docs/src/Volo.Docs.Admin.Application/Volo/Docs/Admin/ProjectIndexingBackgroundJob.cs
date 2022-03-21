using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;
using Volo.Docs.Documents;
using Volo.Docs.Documents.FullSearch.Elastic;
using Volo.Docs.Localization;
using Volo.Docs.Projects;

namespace Volo.Docs.Admin
{
    public class ProjectIndexingBackgroundJob : AsyncBackgroundJob<ProjectIndexingBackgroundJob.ProjectIndexBackgroundWorkerArgs>, ITransientDependency
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IDocumentFullSearch _elasticSearchService;
        private readonly IStringLocalizer<DocsResource> _localizer;

        public ProjectIndexingBackgroundJob(
            IProjectRepository projectRepository,
            IDocumentRepository documentRepository,
            IDocumentFullSearch elasticSearchService,
            IStringLocalizer<DocsResource> localizer)
        {
            _projectRepository = projectRepository;
            _documentRepository = documentRepository;
            _elasticSearchService = elasticSearchService;
            _localizer = localizer;
        }

        public async override Task ExecuteAsync(ProjectIndexBackgroundWorkerArgs args)
        {
            var project = await ReindexProjectAsync(args.ProjectId);
        }

        private async Task<Project> ReindexProjectAsync(Guid projectId)
        {
            var project = await _projectRepository.FindAsync(projectId);
            if (project == null)
            {
                throw new Exception("Cannot find the project with the Id " + projectId);
            }

            var docs = (await _documentRepository.GetListByProjectId(project.Id))
                .Where(doc => doc.FileName != project.NavigationDocumentName && doc.FileName != project.ParametersDocumentName)
                .ToList();

            await _elasticSearchService.DeleteAllByProjectIdAsync(project.Id);

            if (docs.Any())
            {
                await _elasticSearchService.AddOrUpdateManyAsync(docs);
            }

            return project;
        }

        public class ProjectIndexBackgroundWorkerArgs
        {
            public Guid ProjectId { get; set; }

            public ProjectIndexBackgroundWorkerArgs(Guid projectId)
            {
                ProjectId = projectId;
            }
        }
    }

}
