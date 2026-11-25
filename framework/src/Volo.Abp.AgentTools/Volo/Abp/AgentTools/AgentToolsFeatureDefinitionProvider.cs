using Volo.Abp.AgentTools.Localization;
using Volo.Abp.Features;
using Volo.Abp.Localization;
using Volo.Abp.Validation.StringValues;

namespace Volo.Abp.AgentTools;

/// <summary>In this package so every host of the provider and invoker has the definition.</summary>
public class AgentToolsFeatureDefinitionProvider : FeatureDefinitionProvider
{
    public override void Define(IFeatureDefinitionContext context)
    {
        var group = context.AddGroup(AgentToolsFeatures.GroupName, L("Feature:AgentTools"));
        group.AddFeature(
            AgentToolsFeatures.Enable,
            defaultValue: "true",
            displayName: L("Feature:AgentTools.Enable"),
            description: L("Feature:AgentTools.Enable.Description"),
            valueType: new ToggleStringValueType());
    }

    private static LocalizableString L(string name) => LocalizableString.Create<AbpAgentToolsResource>(name);
}
