namespace Volo.Docs.Utils;

public interface IDocsLinkGenerator
{
    string GenerateLink(string projectName, string languageCode, string version, string documentName);
}