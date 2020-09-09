﻿using JetBrains.Annotations;
using Volo.Abp.GlobalFeatures;

namespace Volo.CmsKit.GlobalFeatures
{
    [GlobalFeatureName(Name)]
    public class CommentsFeature : GlobalFeature
    {
        public const string Name = "CmsKit.Comments";

        internal CommentsFeature(
            [NotNull] GlobalCmsKitFeatures cmsKit
        ) : base(cmsKit)
        {

        }
    }
}
