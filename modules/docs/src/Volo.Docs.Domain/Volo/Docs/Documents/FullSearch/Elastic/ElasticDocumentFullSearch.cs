using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Elasticsearch.Net;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nest;
using Volo.Abp;
using Volo.Abp.Domain.Services;
using Volo.Docs.Projects;

namespace Volo.Docs.Documents.FullSearch.Elastic
{
    public class ElasticDocumentFullSearch : DomainService, IDocumentFullSearch
    {
        private readonly IElasticClientProvider _clientProvider;
        private readonly DocsElasticSearchOptions _options;
        private readonly ILogger<ElasticDocumentFullSearch> _logger;
        private readonly IProjectRepository _projectRepository;
        private readonly IDocumentRepository _documentRepository;

        public ElasticDocumentFullSearch(
            IElasticClientProvider clientProvider,
            IOptions<DocsElasticSearchOptions> options,
            IProjectRepository projectRepository,
            IDocumentRepository documentRepository,
            ILogger<ElasticDocumentFullSearch> logger)
        {
            _clientProvider = clientProvider;
            _projectRepository = projectRepository;
            _documentRepository = documentRepository;
            _options = options.Value;

            _logger = logger;
        }

        public virtual async Task CreateIndexIfNeededAsync(CancellationToken cancellationToken = default)
        {
            ValidateElasticSearchEnabled();

            var client = _clientProvider.GetClient();

            var existsResponse = await client.Indices.ExistsAsync(_options.IndexName, ct: cancellationToken);

            HandleError(existsResponse);

            if (!existsResponse.Exists)
            {
                HandleError(await client.Indices.CreateAsync(_options.IndexName, c => c
                    .Map<EsDocument>(m =>
                    {
                        return m
                            .Properties(p => p
                                .Keyword(x => x.Name(d => d.Id))
                                .Keyword(x => x.Name(d => d.ProjectId))
                                .Keyword(x => x.Name(d => d.Name))
                                .Keyword(x => x.Name(d => d.FileName))
                                .Keyword(x => x.Name(d => d.Version))
                                .Keyword(x => x.Name(d => d.LanguageCode))
                                .Text(x => x.Name(d => d.Content)));
                    }), cancellationToken));
            }
        }

        public virtual async Task AddOrUpdateAsync(Document document, CancellationToken cancellationToken = default)
        {
            ValidateElasticSearchEnabled();

            var client = _clientProvider.GetClient();

            var esDocument = new EsDocument
            {
                Id = NormalizeField(document.Id),
                ProjectId = NormalizeField(document.ProjectId),
                Name = document.Name,
                FileName = document.FileName,
                Content = document.Content,
                LanguageCode = NormalizeField(document.LanguageCode),
                Version = NormalizeField(document.Version)
            };

            await client.IndexAsync(esDocument , x=>x.Index(_options.IndexName), cancellationToken);
        }

        public virtual async Task DeleteAsync(Document document, CancellationToken cancellationToken = default)
        {
            ValidateElasticSearchEnabled();

            HandleError(await _clientProvider.GetClient()
                .DeleteAsync(DocumentPath<Document>.Id( NormalizeField(document.Id)), x => x.Index(_options.IndexName), cancellationToken));
        }

        public virtual async Task DeleteAllAsync(CancellationToken cancellationToken = default)
        {
            ValidateElasticSearchEnabled();

            var request = new DeleteByQueryRequest(_options.IndexName)
            {
                Query = new MatchAllQuery()
            };

            HandleError(await _clientProvider.GetClient()
                .DeleteByQueryAsync(request, cancellationToken));
        }

        public virtual async Task DeleteAllByProjectAsync(Project project, CancellationToken cancellationToken = default)
        {
            ValidateElasticSearchEnabled();

            var request = new DeleteByQueryRequest(_options.IndexName)
            {
                Query = new BoolQuery
                {
                    Filter = new QueryContainer[]
                    {
                        new BoolQuery
                        {
                            Must = new QueryContainer[]
                            {
                                new TermQuery
                                {
                                    Field = "projectId",
                                    Value = NormalizeField(project.Id)
                                }
                            }
                        }
                    }
                },
            };

            HandleError(await _clientProvider.GetClient()
                .DeleteByQueryAsync(request, cancellationToken));
        }

        public virtual async Task ReindexDocumentAsync(Document document, CancellationToken cancellationToken = default)
        {
            await DeleteAsync(document, cancellationToken);
            await AddOrUpdateAsync(document, cancellationToken);
        }

        public virtual async Task ReindexProjectAsync(Project project, CancellationToken cancellationToken = default)
        {
            await DeleteAllByProjectAsync(project, cancellationToken);

            var docs = await _documentRepository.GetListByProjectId(project.Id, cancellationToken);

            foreach (var doc in docs)
            {
                if (doc.FileName == project.NavigationDocumentName)
                {
                    continue;
                }

                if (doc.FileName == project.ParametersDocumentName)
                {
                    continue;
                }

                await AddOrUpdateAsync(doc, cancellationToken);
            }
        }

        public virtual async Task ReindexAllAsync(CancellationToken cancellationToken = default)
        {
            await DeleteAllAsync(cancellationToken);

            var docs = await _documentRepository.GetListAsync(cancellationToken: cancellationToken);
            var projects = await _projectRepository.GetListAsync(cancellationToken: cancellationToken);
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

                await AddOrUpdateAsync(doc);
            }

        }

        public virtual async Task<List<EsDocument>> SearchAsync(string context, Project project, string languageCode,
            string version, int? skipCount = null, int? maxResultCount = null,
            CancellationToken cancellationToken = default)
        {
            ValidateElasticSearchEnabled();

            var request = new SearchRequest
            {
                Size = maxResultCount ?? 10,
                From = skipCount ?? 0,
                Query = new BoolQuery
                {
                    Must = new QueryContainer[]
                    {
                        new MatchQuery
                        {
                            Field = "content",
                            Query = context
                        }
                    },
                    Filter = new QueryContainer[]
                    {
                        new BoolQuery
                        {
                            Must = new QueryContainer[]
                            {
                                new TermQuery
                                {
                                    Field = "projectId",
                                    Value = NormalizeField(project.Id)
                                },
                                new TermQuery
                                {
                                    Field = "version",
                                    Value = NormalizeField(version)
                                },
                                new TermQuery
                                {
                                    Field = "languageCode",
                                    Value = NormalizeField(languageCode)
                                }
                            }
                        }
                    }
                },
                Highlight = new Highlight
                {
                    PreTags = new[] { "<highlight>" },
                    PostTags = new[] { "</highlight>" },
                    Fields = new Dictionary<Field, IHighlightField>
                    {
                        {
                            "content", new HighlightField()
                        }
                    }
                }
            };

            var response = await _clientProvider.GetClient().SearchAsync<EsDocument>(request, cancellationToken);

            HandleError(response);

            var docs = new List<EsDocument>();
            foreach (var hit in response.Hits)
            {
                var doc = hit.Source;
                if (hit.Highlight.ContainsKey("content"))
                {
                    doc.Highlight = new List<string>();
                    doc.Highlight.AddRange(hit.Highlight["content"]);
                }
                docs.Add(doc);
            }

            return docs;
        }

        protected virtual void HandleError(IElasticsearchResponse response)
        {
            if (!response.ApiCall.Success)
            {
                _logger.LogError(response.ApiCall.OriginalException,
                    "An error occurred in the elastic search api call.");
            }
        }

        protected virtual void ValidateElasticSearchEnabled()
        {
            if (!_options.Enable)
            {
                throw new BusinessException(DocsDomainErrorCodes.ElasticSearchNotEnabled);
            }
        }

        protected virtual string NormalizeField(Guid field)
        {
            return NormalizeField(field.ToString("N"));
        }

        protected virtual string NormalizeField(string field)
        {
            return field.Replace("-", "").ToLower();
        }
    }
}
