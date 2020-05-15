using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Elasticsearch.Net;
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

        public ElasticDocumentFullSearch(IElasticClientProvider clientProvider, IOptions<DocsElasticSearchOptions> options)
        {
            _clientProvider = clientProvider;
            _options = options.Value;
        }

        public async Task CreateIndexIfNeededAsync(CancellationToken cancellationToken = default)
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

        public async Task AddOrUpdateAsync(Document document, CancellationToken cancellationToken = default)
        {
            ValidateElasticSearchEnabled();

            var client = _clientProvider.GetClient();

            var existsResponse = await client.DocumentExistsAsync(DocumentPath<EsDocument>.Id(document.Id),
                x => x.Index(_options.IndexName), cancellationToken);

            HandleError(existsResponse);

            var esDocument = new EsDocument
            {
                Id = document.Id,
                ProjectId = document.ProjectId,
                Name = document.Name,
                FileName = document.FileName,
                Content = document.Content,
                LanguageCode = document.LanguageCode,
                Version = document.Version
            };

            if (!existsResponse.Exists)
            {
                HandleError(await client.IndexAsync(esDocument,
                    x => x.Id(document.Id).Index(_options.IndexName), cancellationToken));
            }
            else
            {
                HandleError(await client.UpdateAsync(DocumentPath<EsDocument>.Id(document.Id),
                    x => x.Doc(esDocument).Index(_options.IndexName), cancellationToken));
            }

        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            ValidateElasticSearchEnabled();

            HandleError(await _clientProvider.GetClient()
                .DeleteAsync(DocumentPath<Document>.Id(id), x => x.Index(_options.IndexName), cancellationToken));
        }

        public async Task DeleteAllAsync(CancellationToken cancellationToken = default)
        {
            ValidateElasticSearchEnabled();

            var request = new DeleteByQueryRequest(_options.IndexName)
            {
                Query = new MatchAllQuery()
            };

            HandleError(await _clientProvider.GetClient()
                .DeleteByQueryAsync(request, cancellationToken));
        }

        public async Task DeleteAllByProjectIdAsync(Guid projectId, CancellationToken cancellationToken = default)
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
                                    Value = projectId
                                }
                            }
                        }
                    }
                },
            };

            HandleError(await _clientProvider.GetClient()
                .DeleteByQueryAsync(request, cancellationToken));
        }

        public async Task<List<EsDocument>> SearchAsync(string context, Guid projectId, string languageCode,
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
                                    Value = projectId
                                },
                                new TermQuery
                                {
                                    Field = "version",
                                    Value = version
                                },
                                new TermQuery
                                {
                                    Field = "languageCode",
                                    Value = languageCode
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

        protected void HandleError(IElasticsearchResponse response)
        {
            if (!response.ApiCall.Success)
            {
                throw response.ApiCall.OriginalException;
            }
        }

        protected void ValidateElasticSearchEnabled()
        {
            if (!_options.Enable)
            {
                throw new BusinessException(DocsDomainErrorCodes.ElasticSearchNotEnabled);
            }
        }
    }
}
