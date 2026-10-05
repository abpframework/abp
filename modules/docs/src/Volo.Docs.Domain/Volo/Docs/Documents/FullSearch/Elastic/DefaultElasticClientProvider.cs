using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Elastic.Clients.Elasticsearch;
using Volo.Abp.DependencyInjection;

namespace Volo.Docs.Documents.FullSearch.Elastic
{
    public class DefaultElasticClientProvider : IElasticClientProvider, ISingletonDependency
    {
        protected readonly DocsElasticSearchOptions Options;
        protected readonly IConfiguration Configuration;

        public DefaultElasticClientProvider(IOptions<DocsElasticSearchOptions> options, IConfiguration configuration)
        {
            Configuration = configuration;
            Options = options.Value;
        }

        public virtual ElasticsearchClient GetClient()
        {
            var node = new Uri(Configuration["ElasticSearch:Url"]);
            var settings = new ElasticsearchClientSettings(node).DefaultIndex(Options.IndexName);
            return new ElasticsearchClient(Options.Authenticate(settings));
        }
    }
}
