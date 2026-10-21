using System;
using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using Volo.Abp;

namespace Volo.Docs.Documents.FullSearch.Elastic
{
    public class DocsElasticSearchOptions
    {
        public bool Enable { get; set; }

        public string IndexName { get; set; }

        protected Action<ElasticsearchClientSettings> AuthenticationAction { get; set; }

        public DocsElasticSearchOptions()
        {
            Enable = false;
            IndexName = "abp_documents";
        }

        public DocsElasticSearchOptions UseBasicAuthentication(string username, string password)
        {
            Check.NotNullOrEmpty(username, nameof(username));
            Check.NotNullOrEmpty(password, nameof(password));

            AuthenticationAction = settings =>
            {
                settings.Authentication(new BasicAuthentication(username, password));
            };

            return this;
        }

        public DocsElasticSearchOptions UseApiKeyAuthentication(string id, string apiKey)
        {
            Check.NotNullOrEmpty(id, nameof(id));
            Check.NotNullOrEmpty(apiKey, nameof(apiKey));

            AuthenticationAction = settings =>
            {
                settings.Authentication(new Base64ApiKey(id, apiKey));
            };

            return this;
        }

        public ElasticsearchClientSettings Authenticate(ElasticsearchClientSettings connectionSettings)
        {
            AuthenticationAction?.Invoke(connectionSettings);
            return connectionSettings;
        }
    }
}
