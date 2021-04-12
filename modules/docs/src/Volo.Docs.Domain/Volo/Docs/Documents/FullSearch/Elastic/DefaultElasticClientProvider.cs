using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Nest;
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

        public virtual IElasticClient GetClient()
        {
            var node = new Uri(Configuration["ElasticSearch:Url"]);
            var settings = new ConnectionSettings(node).DefaultIndex(Options.IndexName);
            return new ElasticClient(Options.Authenticate(settings));
        }
    }
}
