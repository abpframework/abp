using Volo.Abp.Settings;

namespace DashboardDemo.Settings
{
    public class DashboardDemoSettingDefinitionProvider : SettingDefinitionProvider
    {
        public override void Define(ISettingDefinitionContext context)
        {
            //Define your own settings here. Example:
            //context.Add(new SettingDefinition(DashboardDemoSettings.MySetting1));
        }
    }
}
