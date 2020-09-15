using System;
using Volo.Abp.Features;
using Volo.Abp.Identity.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Validation.StringValues;

namespace Volo.Abp.Identity.Features
{
    public class IdentityFeatureDefinitionProvider : FeatureDefinitionProvider
    {
        public override void Define(IFeatureDefinitionContext context)
        {
            var group = context.AddGroup(IdentityFeature.GroupName, L("Feature:IdentityGroup"));

            group.AddFeature(IdentityFeature.TwoFactor,
                IdentityTwoFactorBehaviour.Optional.ToString(),
                L("Feature:TwoFactor"),
                L("Feature:TwoFactorDescription"),
                new SelectionStringValueType
                {
                    ItemSource = new StaticSelectionStringValueItemSource(
                        new LocalizableSelectionStringValueItem
                        {
                            Value = IdentityTwoFactorBehaviour.Optional.ToString(),
                            DisplayText = GetTwoFactorBehaviourLocalizableStringInfo("Feature:TwoFactor.Optional")
                        },
                        new LocalizableSelectionStringValueItem
                        {
                            Value = IdentityTwoFactorBehaviour.Disabled.ToString(),
                            DisplayText = GetTwoFactorBehaviourLocalizableStringInfo("Feature:TwoFactor.Disabled")
                        },
                        new LocalizableSelectionStringValueItem
                        {
                            Value = IdentityTwoFactorBehaviour.Forced.ToString(),
                            DisplayText = GetTwoFactorBehaviourLocalizableStringInfo("Feature:TwoFactor.Forced")
                        }
                    )
                });
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<IdentityResource>(name);
        }

        private static LocalizableStringInfo GetTwoFactorBehaviourLocalizableStringInfo(string key)
        {
            return new LocalizableStringInfo(LocalizationResourceNameAttribute.GetName(typeof(IdentityResource)), key);
        }
    }
}
