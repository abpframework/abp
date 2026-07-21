using Volo.Abp.Features;
using Volo.Abp.Validation.StringValues;

namespace Volo.Abp.FeatureManagement;

public class TestFeatureDefinitionProvider : FeatureDefinitionProvider
{
    public const string SocialLogins = "SocialLogins";
    public const string EmailSupport = "EmailSupport";
    public const string EmailSupportMaxNumber = "EmailSupportMaxNumber";
    public const string DailyAnalysis = "DailyAnalysis";
    public const string UserCount = "UserCount";
    public const string ProjectCount = "ProjectCount";
    public const string BackupCount = "BackupCount";
    public const string TenantOnlyFeature = "TenantOnlyFeature";
    public const string TenantOnlyParentFeature = "TenantOnlyParentFeature";
    public const string OrphanChildOfTenantOnly = "OrphanChildOfTenantOnly";
    public const string EditionOnlyFeature = "EditionOnlyFeature";

    public override void Define(IFeatureDefinitionContext context)
    {
        var group = context.AddGroup("TestGroup");

        group.AddFeature(
            SocialLogins,
            valueType: new ToggleStringValueType()
        );

        group.AddFeature(
            TenantOnlyFeature,
            defaultValue: false.ToString().ToLowerInvariant(),
            valueType: new ToggleStringValueType()
        ).WithProviders(TenantFeatureValueProvider.ProviderName);

        var tenantOnlyParent = group.AddFeature(
            TenantOnlyParentFeature,
            defaultValue: false.ToString().ToLowerInvariant(),
            valueType: new ToggleStringValueType()
        ).WithProviders(TenantFeatureValueProvider.ProviderName);

        tenantOnlyParent.CreateChild(
            OrphanChildOfTenantOnly,
            defaultValue: false.ToString().ToLowerInvariant(),
            valueType: new ToggleStringValueType());

        group.AddFeature(
            EditionOnlyFeature,
            defaultValue: false.ToString().ToLowerInvariant(),
            valueType: new ToggleStringValueType()
        ).WithProviders(EditionFeatureValueProvider.ProviderName);

        var emailSupport = group.AddFeature(
            EmailSupport,
            "true",
            valueType: new ToggleStringValueType()
        );

        emailSupport.CreateChild(
            EmailSupportMaxNumber,
            "false",
            valueType: new ToggleStringValueType());

        group.AddFeature(
            DailyAnalysis,
            defaultValue: false.ToString().ToLowerInvariant(), //Optional, it is already false by default
            valueType: new ToggleStringValueType()
        );

        group.AddFeature(
            UserCount,
            defaultValue: "1",
            valueType: new FreeTextStringValueType(new NumericValueValidator(1, 1000))
        );

        group.AddFeature(
            ProjectCount,
            defaultValue: "1",
            valueType: new FreeTextStringValueType(new NumericValueValidator(1, 10))
        );

        group.AddFeature(
            BackupCount,
            defaultValue: "0",
            valueType: new FreeTextStringValueType(new NumericValueValidator(0, 10))
        );
    }
}
