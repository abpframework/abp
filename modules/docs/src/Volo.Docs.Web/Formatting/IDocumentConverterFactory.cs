namespace Volo.Docs.Formatting
{
    public interface IDocumentConverterFactory
    {
        IDocumentConverter Create(string format);
    }
}