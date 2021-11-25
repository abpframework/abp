using System;
using Nest;
using Volo.Abp;

namespace Volo.Docs.Documents.FullSearch.Elastic
{
    public class DocsElasticSearchOptions
    {
        public bool Enable { get; set; }

        public string IndexName { get; set; }

        protected Action<ConnectionSettings> AuthenticationAction { get; set; }

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
                settings.BasicAuthentication(username, password);
            };

            return this;
        }

        public DocsElasticSearchOptions UseApiKeyAuthentication(string id, string apiKey)
        {
            Check.NotNullOrEmpty(id, nameof(id));
            Check.NotNullOrEmpty(apiKey, nameof(apiKey));

            AuthenticationAction = settings =>
            {
                settings.ApiKeyAuthentication(id, apiKey);
            };

            return this;
        }

        public ConnectionSettings Authenticate(ConnectionSettings connectionSettings)
        {
            AuthenticationAction?.Invoke(connectionSettings);
            return connectionSettings;
        }
    }
}
