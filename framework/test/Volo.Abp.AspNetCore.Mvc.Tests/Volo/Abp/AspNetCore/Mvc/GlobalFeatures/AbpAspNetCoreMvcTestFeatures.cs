using JetBrains.Annotations;
using Volo.Abp.GlobalFeatures;

namespace Volo.Abp.AspNetCore.Mvc.GlobalFeatures
{
    [GlobalFeatureName(Name)]
    public class AbpAspNetCoreMvcTestFeature1 : Abp.GlobalFeatures.GlobalFeature
    {
        public const string Name = "AbpAspNetCoreMvcTest.Feature1";

        internal  AbpAspNetCoreMvcTestFeature1([NotNull] AbpAspNetCoreMvcTestFeatures abpAspNetCoreMvcTestFeatures)
            : base(abpAspNetCoreMvcTestFeatures)
        {

        }
    }

    public class AbpAspNetCoreMvcTestFeatures : GlobalModuleFeatures
    {
        public const string ModuleName = "AbpAspNetCoreMvcTest";

        public AbpAspNetCoreMvcTestFeature1 Reactions => GetFeature<AbpAspNetCoreMvcTestFeature1>();

        public AbpAspNetCoreMvcTestFeatures([NotNull] GlobalFeatureManager featureManager)
            : base(featureManager)
        {
            AddFeature(new AbpAspNetCoreMvcTestFeature1(this));
        }
    }
}
