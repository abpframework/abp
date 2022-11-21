using Volo.Abp.Features;
using Volo.Abp.GlobalFeatures;
using Volo.Abp.Localization;
using Volo.Abp.Validation.StringValues;
using Volo.CmsKit.GlobalFeatures;
using Volo.CmsKit.Localization;

namespace Volo.CmsKit.Features;
public class CmsKitFeatureDefinitionProvider : FeatureDefinitionProvider
{
    public override void Define(IFeatureDefinitionContext context)
    {
        var group = context.AddGroup(CmsKitFeatures.GroupName,
            L("Feature:CmsKitGroup"));

        if (GlobalFeatureManager.Instance.IsEnabled<BlogsFeature>())
        {
            group.AddFeature(CmsKitFeatures.BlogEnable,
            "true",
            L("Feature:BlogEnable"),
            L("Feature:BlogEnableDescription"),
            new ToggleStringValueType());
        }

        if (GlobalFeatureManager.Instance.IsEnabled<CommentsFeature>())
        {
            group.AddFeature(CmsKitFeatures.CommentEnable,
            "true",
            L("Feature:CommentEnable"),
            L("Feature:CommentEnableDescription"),
            new ToggleStringValueType());
        }

        if (GlobalFeatureManager.Instance.IsEnabled<GlobalResourcesFeature>())
        {
            group.AddFeature(CmsKitFeatures.GlobalResourceEnable,
            "true",
            L("Feature:GlobalResourceEnable"),
            L("Feature:GlobalResourceEnableDescription"),
            new ToggleStringValueType());
        }

        if (GlobalFeatureManager.Instance.IsEnabled<MenuFeature>())
        {
            group.AddFeature(CmsKitFeatures.MenuEnable,
            "true",
            L("Feature:MenuEnable"),
            L("Feature:MenuEnableDescription"),
            new ToggleStringValueType());
        }

        if (GlobalFeatureManager.Instance.IsEnabled<PagesFeature>())
        {
            group.AddFeature(CmsKitFeatures.PageEnable,
            "true",
            L("Feature:PageEnable"),
            L("Feature:PageEnableDescription"),
            new ToggleStringValueType());
        }

        if (GlobalFeatureManager.Instance.IsEnabled<RatingsFeature>())
        {
            group.AddFeature(CmsKitFeatures.RatingEnable,
            "true",
            L("Feature:RatingEnable"),
            L("Feature:RatingEnableDescription"),
            new ToggleStringValueType());
        }

        if (GlobalFeatureManager.Instance.IsEnabled<ReactionsFeature>())
        {
            group.AddFeature(CmsKitFeatures.ReactionEnable,
            "true",
            L("Feature:ReactionEnable"),
            L("Feature:ReactionEnableDescription"),
            new ToggleStringValueType());
        }

        if (GlobalFeatureManager.Instance.IsEnabled<TagsFeature>())
        {
            group.AddFeature(CmsKitFeatures.TagEnable,
            "true",
            L("Feature:TagEnable"),
            L("Feature:TagEnableDescription"),
            new ToggleStringValueType());
        }
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<CmsKitResource>(name);
    }
}
