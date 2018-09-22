namespace Volo.Docs.Formatting
{
    public interface IDocumentConverter
    {
        string Convert(string content);

        string NormalizeLinks(string content, string projectShortName, string version, string documentLocalDirectory);
    }
}