namespace Volo.Docs.Documents
{
    public interface IDocumentSourceFactory
    {
        IDocumentSource Create(string sourceType);
    }
}