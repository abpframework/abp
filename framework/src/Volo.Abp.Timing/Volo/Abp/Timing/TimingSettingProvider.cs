using Volo.Abp.Localization;
using Volo.Abp.Settings;
using Volo.Abp.Timing.Localization.Resources.AbpTiming;

namespace Volo.Abp.Timing;

public class TimingSettingProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        context.Add(
            new SettingDefinition(TimingSettingNames.TimeZone,
                "UTC",
                L("DisplayName:Abp.Timing.Timezone"),
                L("Description:Abp.Timing.Timezone"),
                isVisibleToClients: true)
        );
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<AbpTimingResource>(name);
    }
}
