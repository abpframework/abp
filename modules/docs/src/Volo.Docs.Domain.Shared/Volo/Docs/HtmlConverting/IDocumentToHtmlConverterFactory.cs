namespace Volo.Docs.HtmlConverting;

public interface IDocumentToHtmlConverterFactory
{
    IDocumentToHtmlConverter<TContext> Create<TContext>(string format);
}