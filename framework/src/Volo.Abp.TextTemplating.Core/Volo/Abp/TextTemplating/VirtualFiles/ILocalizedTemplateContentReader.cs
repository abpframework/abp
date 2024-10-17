namespace Volo.Abp.TextTemplating.VirtualFiles;

public interface ILocalizedTemplateContentReader
{
    public string? GetContentOrNull(string? culture);
}
