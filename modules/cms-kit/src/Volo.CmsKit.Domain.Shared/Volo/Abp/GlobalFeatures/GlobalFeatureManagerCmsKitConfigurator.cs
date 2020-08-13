namespace Volo.Abp.GlobalFeatures
{
    public class GlobalFeatureManagerCmsKitConfigurator : GlobalFeatureManagerModuleConfigurator
    {
        public GlobalFeatureManagerCmsKitConfigurator(GlobalFeatureManagerModulesConfigurator modulesConfigurator)
            : base(modulesConfigurator)
        {
            this.Reactions();
            this.Comments();
        }
    }
}
