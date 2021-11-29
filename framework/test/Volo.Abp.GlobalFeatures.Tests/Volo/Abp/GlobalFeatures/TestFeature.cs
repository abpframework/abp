namespace Volo.Abp.GlobalFeatures
{
    [GlobalFeatureName(Name)]
    public class TestFeature : Abp.GlobalFeatures.GlobalFeature
    {
        public const string Name = "TestFeature1";

        internal  TestFeature(TestGlobalModuleFeatures testGlobalModule)
            : base(testGlobalModule)
        {

        }
    }

    public class TestGlobalModuleFeatures : GlobalModuleFeatures
    {
        public const string ModuleName = "GlobalFeatureTest";

        public TestFeature Test => GetFeature<TestFeature>();

        public TestGlobalModuleFeatures(GlobalFeatureManager featureManager)
            : base(featureManager)
        {
            AddFeature(new TestFeature(this));
        }
    }
}
