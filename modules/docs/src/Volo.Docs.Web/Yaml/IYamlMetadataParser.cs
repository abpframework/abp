namespace Volo.Docs.Yaml
{
    public interface IYamlMetadataParser
    {
        YamlMetadata ParseOrNull(string markdown);
    }
}