using Elastic.Clients.Elasticsearch;

namespace Volo.Docs.Documents.FullSearch.Elastic
{
    public interface IElasticClientProvider
    {
        ElasticsearchClient GetClient();
    }
}