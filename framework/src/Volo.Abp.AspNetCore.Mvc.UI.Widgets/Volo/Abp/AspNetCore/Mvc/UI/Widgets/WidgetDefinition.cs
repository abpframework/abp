using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Localization;

namespace Volo.Abp.AspNetCore.Mvc.UI.Widgets;

public class WidgetDefinition
{
    /// <summary>
    /// Unique name of the widget.
    /// </summary>
    [NotNull]
    public string Name { get; }

    [NotNull]
    public WidgetAttribute WidgetAttribute { get; }

    /// <summary>
    /// Display name of the widget.
    /// </summary>
    [NotNull]
    public ILocalizableString DisplayName
    {
        get => _displayName;
        set => _displayName = Check.NotNull(value, nameof(value));
    }
    private ILocalizableString _displayName;

    [NotNull]
    public Type ViewComponentType { get; }

    [NotNull]
    public List<string> RequiredPolicies { get; }

    /// <summary>
    /// Set true to make this Widget available only for authenticated users.
    /// This property is not considered if <see cref="RequiredPolicies"/> is already set.
    /// </summary>
    public bool RequiresAuthentication { get; set; }

    [NotNull]
    public List<WidgetResourceItem> Styles { get; }

    [NotNull]
    public List<WidgetResourceItem> Scripts { get; }

    [CanBeNull]
    public string RefreshUrl { get; set; }

    public bool AutoInitialize { get; set; }

    public WidgetDefinition(
        [NotNull] Type viewComponentType,
        [CanBeNull] ILocalizableString displayName = null)
    {
        ViewComponentType = Check.NotNull(viewComponentType, nameof(viewComponentType));

        WidgetAttribute = WidgetAttribute.Get(viewComponentType);
        Name = GetWidgetName(viewComponentType);
        DisplayName = displayName ?? GetDisplayName(WidgetAttribute, Name);
        RequiredPolicies = GetRequiredPolicies(WidgetAttribute);
        RequiresAuthentication = WidgetAttribute.RequiresAuthentication;
        Styles = GetStyles(WidgetAttribute);
        Scripts = GetScripts(WidgetAttribute);
        RefreshUrl = WidgetAttribute.RefreshUrl;
        AutoInitialize = WidgetAttribute.AutoInitialize;
    }

    private static List<WidgetResourceItem> GetStyles(WidgetAttribute widgetAttribute)
    {
        var styles = new List<WidgetResourceItem>();

        if (!widgetAttribute.StyleTypes.IsNullOrEmpty())
        {
            styles.AddRange(widgetAttribute.StyleTypes.Select(type => new WidgetResourceItem(type)));
        }

        if (!widgetAttribute.StyleFiles.IsNullOrEmpty())
        {
            styles.AddRange(widgetAttribute.StyleFiles.Select(src => new WidgetResourceItem(src)));
        }

        return styles;
    }

    private static List<WidgetResourceItem> GetScripts(WidgetAttribute widgetAttribute)
    {
        var scripts = new List<WidgetResourceItem>();

        if (!widgetAttribute.ScriptTypes.IsNullOrEmpty())
        {
            scripts.AddRange(widgetAttribute.ScriptTypes.Select(type => new WidgetResourceItem(type)));
        }

        if (!widgetAttribute.ScriptFiles.IsNullOrEmpty())
        {
            scripts.AddRange(widgetAttribute.ScriptFiles.Select(src => new WidgetResourceItem(src)));
        }

        return scripts;
    }

    private static List<string> GetRequiredPolicies(WidgetAttribute widgetAttribute)
    {
        var policies = new List<string>();

        if (!widgetAttribute.RequiredPolicies.IsNullOrEmpty())
        {
            policies.AddRange(widgetAttribute.RequiredPolicies);
        }

        return policies;
    }

    private static string GetWidgetName(Type viewComponentType)
    {
        var viewComponentAttr = viewComponentType
            .GetCustomAttributes(typeof(ViewComponentAttribute), true)
            .FirstOrDefault() as ViewComponentAttribute;

        if (viewComponentAttr?.Name != null)
        {
            return viewComponentAttr.Name;
        }

        return viewComponentType.Name.RemovePostFix("ViewComponent");
    }

    private static ILocalizableString GetDisplayName(WidgetAttribute widgetAttribute, string widgetName)
    {
        if (widgetAttribute.DisplayName == null)
        {
            return new FixedLocalizableString(widgetName);
        }

        if (widgetAttribute.DisplayNameResource == null)
        {
            return new FixedLocalizableString(widgetAttribute.DisplayName);
        }

        return new LocalizableString(widgetAttribute.DisplayNameResource, widgetAttribute.DisplayName);
    }

    public WidgetDefinition WithRequiredPolicies(params string[] policyNames)
    {
        foreach (var policyName in policyNames)
        {
            RequiredPolicies.Add(policyName);
        }

        return this;
    }

    /// <summary>
    /// Set true to make this Widget available only for authenticated users.
    /// This value is not considered if <see cref="RequiredPolicies"/> is already set.
    /// </summary>
    public WidgetDefinition WithRequiresAuthentication(bool value = true)
    {
        RequiresAuthentication = value;
        return this;
    }

    public WidgetDefinition WithStyles(params string[] files)
    {
        return WithResources(Styles, files);
    }

    public WidgetDefinition WithStyles(params Type[] bundleContributorTypes)
    {
        return WithResources(Styles, bundleContributorTypes);
    }

    public WidgetDefinition WithScripts(params string[] files)
    {
        return WithResources(Scripts, files);
    }

    public WidgetDefinition WithScripts(params Type[] bundleContributorTypes)
    {
        return WithResources(Scripts, bundleContributorTypes);
    }

    public WidgetDefinition WithRefreshUrl(string refreshUrl)
    {
        RefreshUrl = refreshUrl;
        return this;
    }

    private WidgetDefinition WithResources(List<WidgetResourceItem> resourceItems, Type[] bundleContributorTypes)
    {
        if (bundleContributorTypes.IsNullOrEmpty())
        {
            return this;
        }

        foreach (var bundleContributorType in bundleContributorTypes)
        {
            resourceItems.Add(new WidgetResourceItem(bundleContributorType));
        }

        return this;
    }

    private WidgetDefinition WithResources(List<WidgetResourceItem> resourceItems, string[] files)
    {
        if (files.IsNullOrEmpty())
        {
            return this;
        }

        foreach (var file in files)
        {
            resourceItems.Add(new WidgetResourceItem(file));
        }

        return this;
    }
}
