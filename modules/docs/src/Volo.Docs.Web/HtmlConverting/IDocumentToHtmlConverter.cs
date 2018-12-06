namespace Volo.Docs.HtmlConverting
{
    public interface IDocumentToHtmlConverter
    {
        string Convert(string content);

        string NormalizeLinks(
            string content,
            string projectShortName,
            string version,
            string documentLocalDirectory
        );
    }
}