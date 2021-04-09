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

            Authenticate(settings);

            return new ElasticClient(settings);
        }

        protected virtual void Authenticate(ConnectionSettings connectionSettings)
        {
            switch (Options.AuthenticationMode)
            {
                case DocsElasticSearchOptions.ElasticSearchAuthenticationMode.Basic:
                    var basicAuth = Options.BasicAuthentication;
                    connectionSettings.BasicAuthentication(basicAuth.Username, basicAuth.Password);
                    break;
                case DocsElasticSearchOptions.ElasticSearchAuthenticationMode.ApiKey:
                    var apiKeyAuth = Options.ApiKeyAuthentication;
                    connectionSettings.BasicAuthentication(apiKeyAuth.Id, apiKeyAuth.ApiKey);
                    break;
            }
        }
    }
}