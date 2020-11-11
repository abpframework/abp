namespace Volo.Abp.ObjectExtending.Modularity
{
    public class ExtensionPropertyLookupConfiguration
    {
        public string Url { get; set; }
        public string ResultListPropertyName { get; set; } = "items";
        public string DisplayPropertyName { get; set; } = "text";
        public string ValuePropertyName { get; set; } = "id";
    }
}