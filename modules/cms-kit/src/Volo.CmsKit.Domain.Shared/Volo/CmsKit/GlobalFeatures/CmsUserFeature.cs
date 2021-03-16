using JetBrains.Annotations;
using Volo.Abp.GlobalFeatures;

namespace Volo.CmsKit.GlobalFeatures
{
    [GlobalFeatureName(Name)]
    public class CmsUserFeature : GlobalFeature
    {
        public const string Name = "CmsKit.User";

        internal CmsUserFeature([NotNull] GlobalModuleFeatures module) : base(module)
        {
        }
    }
}
