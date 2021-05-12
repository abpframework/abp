using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;
using Volo.Docs.Admin.Notification;
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
        private readonly IUiNotificationClient _notificationClient;
        private readonly IStringLocalizer<DocsResource> _localizer;

        public ProjectIndexingBackgroundJob(
            IProjectRepository projectRepository,
            IDocumentRepository documentRepository,
            IDocumentFullSearch elasticSearchService,
            IUiNotificationClient notificationClient,
            IStringLocalizer<DocsResource> localizer)
        {
            _projectRepository = projectRepository;
            _documentRepository = documentRepository;
            _elasticSearchService = elasticSearchService;
            _notificationClient = notificationClient;
            _localizer = localizer;
        }

        public override async Task ExecuteAsync(ProjectIndexBackgroundWorkerArgs args)
        {
            var project = await ReindexProjectAsync(args.ProjectId);
            await _notificationClient.SendNotification(_localizer.GetString("SuccessfullyReIndexProject", project.Name));
        }

        private async Task<Project> ReindexProjectAsync(Guid projectId)
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

            return project;
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