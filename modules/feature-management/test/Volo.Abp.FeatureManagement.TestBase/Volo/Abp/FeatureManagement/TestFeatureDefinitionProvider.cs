using Volo.Abp.Features;
using Volo.Abp.Validation.StringValues;

namespace Volo.Abp.FeatureManagement
{
    public class TestFeatureDefinitionProvider : FeatureDefinitionProvider
    {
        public const string SocialLogins = "SocialLogins";
        public const string EmailSupport = "EmailSupport";
        public const string DailyAnalysis = "DailyAnalysis";
        public const string UserCount = "UserCount";
        public const string ProjectCount = "ProjectCount";
        public const string BackupCount = "BackupCount";

        public override void Define(IFeatureDefinitionContext context)
        {
            var group = context.AddGroup("TestGroup");

            group.AddFeature(
                SocialLogins,
                valueType: new ToggleStringValueType()
            );

            group.AddFeature(
                EmailSupport,
                valueType: new ToggleStringValueType()
            );

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
}
