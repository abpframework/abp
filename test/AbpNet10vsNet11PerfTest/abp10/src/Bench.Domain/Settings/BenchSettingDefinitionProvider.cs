using Volo.Abp.Settings;

namespace Bench.Settings;

public class BenchSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(BenchSettings.MySetting1));
    }
}
