using Volo.Abp.Settings;

namespace Volo.CmsKit.Settings;

public class CmsKitSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        context.Add(new SettingDefinition(CmsKitSettings.Comments.RequireApprovement, "false"));
    }
}
