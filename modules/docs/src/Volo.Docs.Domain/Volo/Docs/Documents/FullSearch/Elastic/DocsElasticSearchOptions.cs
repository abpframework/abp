namespace Volo.Docs.Documents.FullSearch.Elastic
{
    public class DocsElasticSearchOptions
    {
        public DocsElasticSearchOptions()
        {
            Enable = false;
            IndexName = "abp_documents";
        }

        public bool Enable { get; set; }

        public string IndexName { get; set; }
    }
}
