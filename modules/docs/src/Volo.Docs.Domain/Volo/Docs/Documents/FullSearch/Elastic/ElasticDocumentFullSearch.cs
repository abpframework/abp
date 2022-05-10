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

namespace Volo.Docs.Documents.FullSearch.Elastic
{
    public class ElasticDocumentFullSearch : DomainService, IDocumentFullSearch
    {
        private readonly IElasticClientProvider _clientProvider;
        private readonly DocsElasticSearchOptions _options;
        private readonly ILogger<ElasticDocumentFullSearch> _logger;

        public ElasticDocumentFullSearch(IElasticClientProvider clientProvider,
            IOptions<DocsElasticSearchOptions> options,
            ILogger<ElasticDocumentFullSearch> logger)
        {
            _clientProvider = clientProvider;
            _logger = logger;
            _options = options.Value;
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

            HandleError(await client.IndexAsync(esDocument, x => x.Index(_options.IndexName), cancellationToken));
        }

        public virtual async Task AddOrUpdateManyAsync(IEnumerable<Document> documents, CancellationToken cancellationToken = default)
        {
            var client = _clientProvider.GetClient();

            var esDocuments = documents.Select(x => new EsDocument {
                Id = NormalizeField(x.Id),
                ProjectId = NormalizeField(x.ProjectId),
                Name = x.Name,
                FileName = x.FileName,
                Content = x.Content,
                LanguageCode = NormalizeField(x.LanguageCode),
                Version = NormalizeField(x.Version)
            });

            HandleError(await client.IndexManyAsync(esDocuments, _options.IndexName, cancellationToken));
        }

        public virtual async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            HandleError(await _clientProvider.GetClient()
                .DeleteAsync(DocumentPath<Document>.Id(NormalizeField(id)), x => x.Index(_options.IndexName), cancellationToken));
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

        public virtual async Task DeleteAllByProjectIdAsync(Guid projectId, CancellationToken cancellationToken = default)
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
                                    Value = NormalizeField(projectId)
                                }
                            }
                        }
                    }
                },
            };

            HandleError(await _clientProvider.GetClient()
                .DeleteByQueryAsync(request, cancellationToken));
        }

        public virtual async Task<List<EsDocument>> SearchAsync(string context, Guid projectId, string languageCode,
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
                                    Value = NormalizeField(projectId)
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

        public virtual void ValidateElasticSearchEnabled()
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
