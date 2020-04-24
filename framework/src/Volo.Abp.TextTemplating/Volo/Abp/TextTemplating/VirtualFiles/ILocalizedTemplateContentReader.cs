namespace Volo.Abp.TextTemplating.VirtualFiles
{
    public interface ILocalizedTemplateContentReader
    {
        public string GetContent(string culture, string defaultCultureName = null);
    }
}