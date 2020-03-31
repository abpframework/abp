using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Caching;
using Volo.Docs.Documents;
using Volo.Docs.Documents.FullSearch.Elastic;
using Volo.Docs.Projects;
using Volo.Extensions;

namespace Volo.Docs.Admin.Documents
{
    [Authorize(DocsAdminPermissions.Documents.Default)]
    public class DocumentAdminAppService : ApplicationService, IDocumentAdminAppService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IDocumentRepository _documentRepository;
        private readonly IDocumentSourceFactory _documentStoreFactory;
        private readonly IDistributedCache<DocumentUpdateInfo> _documentUpdateCache;
        private readonly IDocumentFullSearch _documentFullSearch;

        public DocumentAdminAppService(IProjectRepository projectRepository,
            IDocumentRepository documentRepository,
            IDocumentSourceFactory documentStoreFactory,
            IDistributedCache<DocumentUpdateInfo> documentUpdateCache,
            IDocumentFullSearch documentFullSearch)
        {
            _projectRepository = projectRepository;
            _documentRepository = documentRepository;
            _documentStoreFactory = documentStoreFactory;
            _documentUpdateCache = documentUpdateCache;
            _documentFullSearch = documentFullSearch;
        }

        public async Task PullAllAsync(PullAllDocumentInput input)
        {
            var project = await _projectRepository.GetAsync(input.ProjectId);

            var navigationDocument = await GetDocumentAsync(
                project,
                project.NavigationDocumentName,
                input.LanguageCode,
                input.Version
            );

            if (!JsonConvertExtensions.TryDeserializeObject<NavigationNode>(navigationDocument.Content, out var navigation))
            {
                throw new UserFriendlyException($"Cannot validate navigation file '{project.NavigationDocumentName}' for the project {project.Name}.");
            }

            var leafs = navigation.Items.GetAllNodes(x => x.Items)
                .Where(x => x.IsLeaf && !x.Path.IsNullOrWhiteSpace())
                .ToList();

            var source = _documentStoreFactory.Create(project.DocumentStoreType);

            var documents = new List<Document>();
            foreach (var leaf in leafs)
            {
                var sourceDocument =
                    await source.GetDocumentAsync(project, leaf.Path, input.LanguageCode, input.Version);
                documents.Add(sourceDocument);
            }

            foreach (var document in documents)
            {
                await _documentRepository.DeleteAsync(document.ProjectId, document.Name,
                    document.LanguageCode,
                    document.Version);

                await _documentRepository.InsertAsync(document, true);
                await UpdateDocumentUpdateInfoCache(document);
            }
        }

        public async Task PullAsync(PullDocumentInput input)
        {
            var project = await _projectRepository.GetAsync(input.ProjectId);

            var source = _documentStoreFactory.Create(project.DocumentStoreType);
            var sourceDocument = await source.GetDocumentAsync(project, input.Name, input.LanguageCode, input.Version);

            await _documentRepository.DeleteAsync(sourceDocument.ProjectId, sourceDocument.Name,
                sourceDocument.LanguageCode, sourceDocument.Version);
            await _documentRepository.InsertAsync(sourceDocument, true);
            await UpdateDocumentUpdateInfoCache(sourceDocument);
        }

        public async Task ReindexAsync()
        {
            var docs = await _documentRepository.GetListAsync();
            var projects = await _projectRepository.GetListAsync();
            foreach (var doc in docs)
            {
                var project = projects.FirstOrDefault(x => x.Id == doc.ProjectId);
                if (project == null)
                {
                    continue;
                }

                if (doc.FileName == project.NavigationDocumentName)
                {
                    continue;
                }

                if (doc.FileName == project.ParametersDocumentName)
                {
                    continue;
                }

                await _documentFullSearch.AddOrUpdateAsync(doc);
            }
        }

        private async Task UpdateDocumentUpdateInfoCache(Document document)
        {
            var cacheKey = $"DocumentUpdateInfo{document.ProjectId}#{document.Name}#{document.LanguageCode}#{document.Version}";
            await _documentUpdateCache.SetAsync(cacheKey, new DocumentUpdateInfo
            {
                Name = document.Name,
                CreationTime = document.CreationTime,
                LastUpdatedTime = document.LastUpdatedTime
            });
        }

        private async Task<Document> GetDocumentAsync(
            Project project,
            string documentName,
            string languageCode,
            string version)
        {
            version = string.IsNullOrWhiteSpace(version) ? project.LatestVersionBranchName : version;
            var source = _documentStoreFactory.Create(project.DocumentStoreType);
            var document = await source.GetDocumentAsync(project, documentName, languageCode, version);
            return document;
        }
    }
}