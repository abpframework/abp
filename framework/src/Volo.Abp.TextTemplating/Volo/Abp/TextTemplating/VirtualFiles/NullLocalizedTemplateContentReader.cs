namespace Volo.Abp.TextTemplating.VirtualFiles
{
    public class NullLocalizedTemplateContentReader : ILocalizedTemplateContentReader
    {
        public static NullLocalizedTemplateContentReader Instance { get; } = new NullLocalizedTemplateContentReader();

        private NullLocalizedTemplateContentReader()
        {
            
        }

        public string GetContent(
            string culture,
            string defaultCultureName = null)
        {
            return null;
        }
    }
}