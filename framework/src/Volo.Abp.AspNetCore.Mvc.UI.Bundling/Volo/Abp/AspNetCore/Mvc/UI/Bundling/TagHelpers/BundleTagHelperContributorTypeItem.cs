using System;
using JetBrains.Annotations;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling.TagHelpers
{
    public class BundleTagHelperContributorTypeItem : BundleTagHelperItem
    {
        [NotNull]
        public Type Type { get; }

        public BundleTagHelperContributorTypeItem([NotNull] Type type)
        {
            Type = Check.NotNull(type, nameof(type));
        }

        public override string ToString()
        {
            return Type.FullName;
        }

        public override void AddToConfiguration(BundleConfiguration configuration)
        {
            configuration.AddContributors(Type);
        }
    }
}