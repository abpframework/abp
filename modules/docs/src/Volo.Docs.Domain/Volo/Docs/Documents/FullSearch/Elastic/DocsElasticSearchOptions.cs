namespace Volo.Docs.Documents.FullSearch.Elastic
{
    public class DocsElasticSearchOptions
    {
        public DocsElasticSearchOptions()
        {
            Enable = false;
            IndexName = "abp_documents";
            AuthenticationMode = ElasticSearchAuthenticationMode.None;
        }

        public bool Enable { get; set; }

        public string IndexName { get; set; }

        public ElasticSearchAuthenticationMode AuthenticationMode { get; set; }

        public DocsElasticSearchBasicAuthenticationOptions BasicAuthentication { get; set; }
        public DocsElasticSearchApiKeyAuthenticationOptions ApiKeyAuthentication { get; set; }

        public enum ElasticSearchAuthenticationMode
        {
            None = 0,
            Basic = 1,
            ApiKey = 2
        }
    }
}
