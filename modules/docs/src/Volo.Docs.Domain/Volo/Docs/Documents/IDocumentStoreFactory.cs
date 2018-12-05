namespace Volo.Docs.Documents
{
    public interface IDocumentStoreFactory
    {
        IDocumentStore Create(string storeType);
    }
}