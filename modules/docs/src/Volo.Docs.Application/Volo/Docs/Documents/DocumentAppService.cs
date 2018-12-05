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

        public virtual async Task<NavigationWithDetailsDto> GetNavigationDocumentAsync(GetNavigationDocumentInput input)
        {
            var project = await _projectRepository.GetAsync(input.ProjectId);
            var documentDto = await GetDocumentWithDetailsDto(
                project,
                project.NavigationDocumentName,
                input.Version
            );

            return ObjectMapper.Map<DocumentWithDetailsDto, NavigationWithDetailsDto>(documentDto);
        }

        protected virtual async Task<DocumentWithDetailsDto> GetDocumentWithDetailsDto(
            Project project, 
            string documentName, 
            string version)
        {
            var documentStore = _documentStoreFactory.Create(project.DocumentStoreType);
            var document = await documentStore.Find(project, documentName, version);

            var dto = ObjectMapper.Map<Document, DocumentWithDetailsDto>(document);
            dto.Project = ObjectMapper.Map<Project, ProjectDto>(project);

            return dto;
        }
    }
}