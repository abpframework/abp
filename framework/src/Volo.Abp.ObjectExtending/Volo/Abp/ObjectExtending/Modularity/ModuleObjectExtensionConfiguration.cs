namespace Volo.Abp.ObjectExtending.Modularity
{
    public class ModuleObjectExtensionConfiguration
    {
        public ModuleEntityObjectExtensionConfigurationDictionary Entities { get; set; }

        public ModuleObjectExtensionConfiguration()
        {
            Entities = new ModuleEntityObjectExtensionConfigurationDictionary();
        }
    }
}