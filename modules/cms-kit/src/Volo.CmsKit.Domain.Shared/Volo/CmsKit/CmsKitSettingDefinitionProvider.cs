using Volo.Abp.Settings;

namespace Volo.CmsKit;

public class CmsKitSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        context.Add(new SettingDefinition(CmsKitSettings.Comments.RequireApprovement, "false", null, null, true));
    }
}
