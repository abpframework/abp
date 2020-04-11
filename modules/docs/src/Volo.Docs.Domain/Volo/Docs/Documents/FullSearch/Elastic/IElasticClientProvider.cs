using Nest;

namespace Volo.Docs.Documents.FullSearch.Elastic
{
    public interface IElasticClientProvider
    {
        IElasticClient GetClient();
    }
}