using System;
using System.Collections.Generic;
using System.Reflection;
using Volo.Abp;
using Volo.Abp.GlobalFeatures;

namespace Volo.CmsKit.Web.Contents;

public class CmsKitContentWidgetOptions
{
    public Dictionary<string, ContentWidgetConfig> WidgetConfigs { get; }

    public CmsKitContentWidgetOptions()
    {
        WidgetConfigs = new();
    }

    public void AddWidget(string widgetType, string widgetName, string parameterWidgetName = null)
    {
        var config = new ContentWidgetConfig(widgetName, parameterWidgetName);
        WidgetConfigs.Add(widgetType, config);
    }

    public void AddWidgetIfFeatureEnabled(Type globalFeatureType, string widgetType, string widgetName, string parameterWidgetName = null)
    {
        Check.NotNull(globalFeatureType, nameof(globalFeatureType));

        if (globalFeatureType.GetCustomAttribute<GlobalFeatureNameAttribute>() == null)
        {
            throw new ArgumentException($"The type {globalFeatureType.Name} must have a {nameof(GlobalFeatureNameAttribute)} attribute.", nameof(globalFeatureType));
        }

        if (!GlobalFeatureManager.Instance.IsEnabled(globalFeatureType))
        {
            return;
        }

        AddWidget(widgetType, widgetName, parameterWidgetName);
    }
}