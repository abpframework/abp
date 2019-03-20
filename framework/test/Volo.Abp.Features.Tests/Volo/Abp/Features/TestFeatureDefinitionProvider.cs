namespace Volo.Abp.Features
{
    public class TestFeatureDefinitionProvider : FeatureDefinitionProvider
    {
        public override void Define(IFeatureDefinitionContext context)
        {
            var group = context.AddGroup("Test Group");
            group.AddFeature("BooleanTestFeature1");
            group.AddFeature("BooleanTestFeature2");
            group.AddFeature("IntegerTestFeature1", defaultValue: "1");
        }
    }
}
