namespace Volo.Docs.Formatting
{
    public interface IDocumentFormattingFactory
    {
        IDocumentFormatting Create(string format);
    }
}