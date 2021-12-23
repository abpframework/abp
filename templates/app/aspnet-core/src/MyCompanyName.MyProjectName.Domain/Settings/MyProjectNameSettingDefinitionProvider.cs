using Volo.Abp.Settings;

namespace MyCompanyName.MyProjectName.Settings;

public class MyProjectNameSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(MyProjectNameSettings.MySetting1));
    }
}
