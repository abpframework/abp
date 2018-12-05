using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Docs.Projects;

namespace Volo.Docs.Documents
{
    public class DocumentAppService : ApplicationService, IDocumentAppService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IDocumentStoreFactory _documentStoreFactory;

        public DocumentAppService(
            IProjectRepository projectRepository,
            IDocumentStoreFactory documentStoreFactory)
        {
            _projectRepository = projectRepository;
            _documentStoreFactory = documentStoreFactory;
        }

        public async Task<DocumentWithDetailsDto> GetAsync(GetDocumentInput input)
        {
            var project = await _projectRepository.GetAsync(input.ProjectId);
            return await GetDocumentWithDetailsDto(
                project,
                input.Name,
                input.Version
            );
        }

        public async Task<DocumentWithDetailsDto> GetDefaultAsync(GetDefaultDocumentInput input)
        {
            var project = await _projectRepository.GetAsync(input.ProjectId);
            return await GetDocumentWithDetailsDto(
                project,
                project.DefaultDocumentName,
                input.Version
            );
        }

        public virtual async Task<DocumentWithDetailsDto> GetNavigationDocumentAsync(GetNavigationDocumentInput input)
        {
            var project = await _projectRepository.GetAsync(input.ProjectId);
            return await GetDocumentWithDetailsDto(
                project,
                project.NavigationDocumentName,
                input.Version
            );
        }

        protected virtual async Task<DocumentWithDetailsDto> GetDocumentWithDetailsDto(
            Project project, 
            string documentName, 
            string version)
        {
            var documentStore = _documentStoreFactory.Create(project.DocumentStoreType);
            var document = await documentStore.FindDocument(project, documentName, version);

            var dto = ObjectMapper.Map<Document, DocumentWithDetailsDto>(document);
            dto.Project = ObjectMapper.Map<Project, ProjectDto>(project);

            return dto;
        }
    }
}