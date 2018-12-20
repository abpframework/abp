using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Volo.Abp.Application.Services;
using Volo.Abp.Caching;
using Volo.Docs.Projects;

namespace Volo.Docs.Documents
{
    public class DocumentAppService : ApplicationService, IDocumentAppService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IDocumentStoreFactory _documentStoreFactory;
        protected IDistributedCache<DocumentWithDetailsDto> DocumentCache { get; }
        protected IDistributedCache<DocumentResourceDto> ResourceCache { get; }

        public DocumentAppService(
            IProjectRepository projectRepository,
            IDocumentStoreFactory documentStoreFactory, 
            IDistributedCache<DocumentWithDetailsDto> documentCache, 
            IDistributedCache<DocumentResourceDto> resourceCache)
        {
            _projectRepository = projectRepository;
            _documentStoreFactory = documentStoreFactory;
            DocumentCache = documentCache;
            ResourceCache = resourceCache;
        }

        public virtual async Task<DocumentWithDetailsDto> GetAsync(GetDocumentInput input)
        {
            var project = await _projectRepository.GetAsync(input.ProjectId);

            return await GetDocumentWithDetailsDto(
                project,
                input.Name,
                input.Version
            );
        }

        public virtual async Task<DocumentWithDetailsDto> GetDefaultAsync(GetDefaultDocumentInput input)
        {
            var project = await _projectRepository.GetAsync(input.ProjectId);

            return await GetDocumentWithDetailsDto(
                project,
                project.DefaultDocumentName,
                input.Version
            );
        }

        public virtual async Task<DocumentWithDetailsDto> GetNavigationAsync(GetNavigationDocumentInput input)
        {
            var project = await _projectRepository.GetAsync(input.ProjectId);

            return await GetDocumentWithDetailsDto(
                project,
                project.NavigationDocumentName,
                input.Version
            );
        }

        public async Task<DocumentResourceDto> GetResourceAsync(GetDocumentResourceInput input)
        {
            var project = await _projectRepository.GetAsync(input.ProjectId);
            var cacheKey = $"Resource@{project.ShortName}#{input.Name}#{input.Version}";

            return await ResourceCache.GetOrAddAsync(
                cacheKey,
                async () =>
                {
                    var store = _documentStoreFactory.Create(project.DocumentStoreType);
                    var documentResource = await store.GetResource(project, input.Name, input.Version);

                    return ObjectMapper.Map<DocumentResource, DocumentResourceDto>(documentResource);
                },
                () => new DistributedCacheEntryOptions
                {
                    SlidingExpiration = TimeSpan.FromMinutes(30) //TODO: Configurable?
                }
            );
        }

        protected virtual async Task<DocumentWithDetailsDto> GetDocumentWithDetailsDto(
            Project project, 
            string documentName, 
            string version)
        {
            var cacheKey = $"Document@{project.ShortName}#{documentName}#{version}";

            return await DocumentCache.GetOrAddAsync(
                cacheKey,
                async () =>
                {
                    var store = _documentStoreFactory.Create(project.DocumentStoreType);
                    var document = await store.GetDocument(project, documentName, version);

                    return CreateDocumentWithDetailsDto(project, document);
                },
                () => new DistributedCacheEntryOptions
                {
                    SlidingExpiration = TimeSpan.FromMinutes(30) //TODO: Configurable?
                }
            );
        }

        protected virtual DocumentWithDetailsDto CreateDocumentWithDetailsDto(Project project, Document document)
        {
            var documentDto = ObjectMapper.Map<Document, DocumentWithDetailsDto>(document);
            documentDto.Project = ObjectMapper.Map<Project, ProjectDto>(project);
            return documentDto;
        }
    }
}