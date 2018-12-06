namespace Volo.Docs.HtmlConverting
{
    public interface IDocumentToHtmlConverterFactory
    {
        IDocumentToHtmlConverter Create(string format);
    }
}