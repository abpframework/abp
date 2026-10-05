using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Core.Search;
using Elastic.Clients.Elasticsearch.IndexManagement;
using Elastic.Clients.Elasticsearch.Mapping;
using Elastic.Clients.Elasticsearch.QueryDsl;
using Elastic.Transport.Products.Elasticsearch;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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

            var existsResponse = await client.Indices.ExistsAsync(_options.IndexName, cancellationToken);

            HandleError(existsResponse);

            if (!existsResponse.Exists)
            {
                var properties = new Properties();
                properties.Add<EsDocument>(d => d.Id, new KeywordProperty());
                properties.Add<EsDocument>(d => d.ProjectId, new KeywordProperty());
                properties.Add<EsDocument>(d => d.Name, new KeywordProperty());
                properties.Add<EsDocument>(d => d.FileName, new KeywordProperty());
                properties.Add<EsDocument>(d => d.Version, new KeywordProperty());
                properties.Add<EsDocument>(d => d.LanguageCode, new KeywordProperty());
                properties.Add<EsDocument>(d => d.Content, new TextProperty());

                HandleError(await client.Indices.CreateAsync(new CreateIndexRequest(_options.IndexName)
                {
                    Mappings = new TypeMapping
                    {
                        Properties = properties
                    }
                }, cancellationToken));
            }
        }

        public virtual async Task AddOrUpdateAsync(Document document, CancellationToken cancellationToken = default)
        {
            var client = _clientProvider.GetClient();
            
            // exist by name, project id, language code and version
            HandleError(await client.DeleteByQueryAsync(new DeleteByQueryRequest(_options.IndexName)
            {
                Query = new BoolQuery
                {
                    Must = new Query[]
                    {
                        new TermQuery { Field = "projectId", Value = NormalizeField(document.ProjectId) },
                        new TermQuery { Field = "languageCode", Value = NormalizeField(document.LanguageCode) },
                        new TermQuery { Field = "version", Value = NormalizeField(document.Version) },
                        new TermQuery { Field = "name", Value = document.Name }
                    }
                }
            }, cancellationToken));

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

            HandleError(await client.IndexAsync(esDocument, _options.IndexName, esDocument.Id, cancellationToken));
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
                .DeleteAsync(_options.IndexName, NormalizeField(id), cancellationToken));
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
                    Filter = new Query[]
                    {
                        new BoolQuery
                        {
                            Must = new Query[]
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

        public virtual async Task<EsDocumentResult> SearchAsync(string context, Guid projectId, string languageCode,
            string version, int? skipCount = null, int? maxResultCount = null,
            CancellationToken cancellationToken = default)
        {
            ValidateElasticSearchEnabled();
            
            Query query;
            // if context starts with " or ends with " then we search for exact match
            if (context.StartsWith("\"") && context.EndsWith("\""))
            {
                context = context.Trim('"');
                
                query = new MatchPhraseQuery
                {
                    Field = "content",
                    Query = context
                };
            }
            else
            {
                query = new MatchQuery
                {
                    Field = "content",
                    Query = context
                };
            }

            var request = new SearchRequest(_options.IndexName)
            {
                Size = maxResultCount ?? 10,
                From = skipCount ?? 0,
                Query = new BoolQuery
                {
                    Must = new Query[]
                    {
                        query,
                    },
                    Filter = new Query[]
                    {
                        new BoolQuery
                        {
                            Must = new Query[]
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
                    Fields = new Dictionary<Field, HighlightField>
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
                if(docs.Any(x => x.Id == doc.Id))
                {
                    continue;
                }


                if (hit.Highlight != null && hit.Highlight.TryGetValue("content", out var highlights))
                {
                    doc.Highlight = new List<string>();
                    doc.Highlight.AddRange(highlights);
                }

                docs.Add(doc);
            }

            return new EsDocumentResult { EsDocuments = docs, TotalCount = response.Total };
        }

        protected virtual void HandleError(ElasticsearchResponse response)
        {
            if (!response.IsValidResponse)
            {
                response.TryGetOriginalException(out var exception);
                _logger.LogError(exception,
                    "An error occurred in the elastic search api call. {ElasticsearchServerError}", response.ElasticsearchServerError);
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
            return field?.Replace("-", "").ToLower();
        }
    }
}
