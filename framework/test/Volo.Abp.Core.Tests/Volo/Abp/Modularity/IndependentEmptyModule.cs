namespace Volo.Abp.Modularity
{
    public class IndependentEmptyModule : TestModuleBase
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            base.PreConfigureServices(context);
            SkipAutoServiceRegistration = true;
        }
    }
}
