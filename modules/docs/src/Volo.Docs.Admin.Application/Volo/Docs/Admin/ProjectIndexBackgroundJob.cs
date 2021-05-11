using System;
using System.Threading.Tasks;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;
using Volo.Docs.Documents;
using Volo.Docs.Documents.FullSearch.Elastic;
using Volo.Docs.Projects;

namespace Volo.Docs.Admin
{
    public class ProjectIndexBackgroundWorkerArgs
    {
        public Guid ProjectId { get; set; }

        public ProjectIndexBackgroundWorkerArgs(Guid projectId)
        {
            ProjectId = projectId;
        }
    }

    public class ProjectIndexBackgroundJob : AsyncBackgroundJob<ProjectIndexBackgroundWorkerArgs>, ITransientDependency
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IDocumentFullSearch _elasticSearchService;

        public ProjectIndexBackgroundJob(
            IProjectRepository projectRepository,
            IDocumentRepository documentRepository,
            IDocumentFullSearch elasticSearchService)
        {
            _projectRepository = projectRepository;
            _documentRepository = documentRepository;
            _elasticSearchService = elasticSearchService;
        }

        public override async Task ExecuteAsync(ProjectIndexBackgroundWorkerArgs args)
        {
            await ReindexProjectAsync(args.ProjectId);
        }

        private async Task ReindexProjectAsync(Guid projectId)
        {
            var project = await _projectRepository.FindAsync(projectId);
            if (project == null)
            {
                throw new Exception("Cannot find the project with the Id " + projectId);
            }

            var docs = await _documentRepository.GetListByProjectId(project.Id);
            await _elasticSearchService.DeleteAllByProjectIdAsync(project.Id);

            foreach (var doc in docs)
            {
                if (ShouldIgnoreDocument(doc, project))
                {
                    continue;
                }

                await _elasticSearchService.AddOrUpdateAsync(doc);
            }
        }

        private static bool ShouldIgnoreDocument(Document doc, Project project)
        {
            if (doc.FileName == project.NavigationDocumentName)
            {
                return true;
            }

            if (doc.FileName == project.ParametersDocumentName)
            {
                return true;
            }

            return false;
        }
    }
}