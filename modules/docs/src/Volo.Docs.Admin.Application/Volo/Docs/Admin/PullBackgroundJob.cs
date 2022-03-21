using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Docs.Documents;
using Volo.Docs.Projects;
using Volo.Extensions;

namespace Volo.Docs.Admin;

public abstract class PullBackgroundJobBase<TArgs> : AsyncBackgroundJob<TArgs>, ITransientDependency
{
    public ILogger<AsyncBackgroundJob<PullBackgroundJobBase<TArgs>>> Logger { get; set; }

    private readonly IProjectRepository _projectRepository;
    private readonly IDocumentRepository _documentRepository;
    private readonly IDocumentSourceFactory _documentStoreFactory;
    private readonly IDistributedCache<DocumentUpdateInfo> _documentUpdateCache;

    public PullBackgroundJobBase(
        IProjectRepository projectRepository,
        IDocumentRepository documentRepository,
        IDocumentSourceFactory documentStoreFactory,
        IDistributedCache<DocumentUpdateInfo> documentUpdateCache)
    {
        _projectRepository = projectRepository;
        _documentRepository = documentRepository;
        _documentStoreFactory = documentStoreFactory;
        _documentUpdateCache = documentUpdateCache;

        Logger = NullLogger<AsyncBackgroundJob<PullBackgroundJobBase<TArgs>>>.Instance;
    }

    protected async Task PullAsync(PullBackgroundJob.PullBackgroundWorkerArgs args)
    {
        var project = await _projectRepository.GetAsync(args.ProjectId);

        var source = _documentStoreFactory.Create(project.DocumentStoreType);
        var sourceDocument = await source.GetDocumentAsync(project, args.Name, args.LanguageCode, args.Version);

        await _documentRepository.DeleteAsync(sourceDocument.ProjectId, sourceDocument.Name, sourceDocument.LanguageCode, sourceDocument.Version);
        await _documentRepository.InsertAsync(sourceDocument, true);
        await UpdateDocumentUpdateInfoCache(sourceDocument);
    }

    protected async Task PullAllAsync(PullAllBackgroundJob.PullAllBackgroundWorkerArgs args)
    {
        var project = await _projectRepository.GetAsync(args.ProjectId);

        var navigationDocument = await GetDocumentAsync(
            project,
            project.NavigationDocumentName,
            args.LanguageCode,
            args.Version
        );

        if (!DocsJsonSerializerHelper.TryDeserialize<NavigationNode>(navigationDocument.Content, out var navigation))
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
            if (leaf.Path.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
                leaf.Path.StartsWith("https://", StringComparison.OrdinalIgnoreCase) ||
                (leaf.Path.StartsWith("{{") && leaf.Path.EndsWith("}}")))
            {
                continue;
            }

            try
            {
                var sourceDocument = await source.GetDocumentAsync(project, leaf.Path, args.LanguageCode, args.Version);
                documents.Add(sourceDocument);
            }
            catch (Exception e)
            {
                Logger.LogException(e);
            }
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

public class PullAllBackgroundJob : PullBackgroundJobBase<PullAllBackgroundJob.PullAllBackgroundWorkerArgs>
{
    public PullAllBackgroundJob(
        IProjectRepository projectRepository,
        IDocumentRepository documentRepository,
        IDocumentSourceFactory documentStoreFactory,
        IDistributedCache<DocumentUpdateInfo> documentUpdateCache)
        : base(projectRepository, documentRepository, documentStoreFactory, documentUpdateCache)
    {
    }

    public async override Task ExecuteAsync(PullAllBackgroundWorkerArgs args)
    {
        await PullAllAsync(args);
    }

    public class PullAllBackgroundWorkerArgs
    {
        public Guid ProjectId { get; set; }

        public string LanguageCode { get; set; }

        public string Version { get; set; }

        public PullAllBackgroundWorkerArgs(Guid projectId, string languageCode, string version)
        {
            ProjectId = projectId;
            LanguageCode = languageCode;
            Version = version;
        }
    }
}

public class PullBackgroundJob : PullBackgroundJobBase<PullBackgroundJob.PullBackgroundWorkerArgs>
{
    public PullBackgroundJob(
        IProjectRepository projectRepository,
        IDocumentRepository documentRepository,
        IDocumentSourceFactory documentStoreFactory,
        IDistributedCache<DocumentUpdateInfo> documentUpdateCache)
        : base(projectRepository, documentRepository, documentStoreFactory, documentUpdateCache)
    {
    }

    public async override Task ExecuteAsync(PullBackgroundWorkerArgs args)
    {
        await PullAsync(args);
    }

    public class PullBackgroundWorkerArgs
    {
        public Guid ProjectId { get; set; }

        public string LanguageCode { get; set; }

        public string Version { get; set; }

        [CanBeNull]
        public string Name { get; set; }

        public PullBackgroundWorkerArgs(Guid projectId, string languageCode, string version, [CanBeNull] string name = null)
        {
            ProjectId = projectId;
            LanguageCode = languageCode;
            Version = version;
            Name = name;
        }
    }
}
