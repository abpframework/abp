using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Volo.Abp.Localization;

namespace Volo.Abp.AspNetCore.Mvc.UI.Widgets
{
    public class WidgetDefinition
    {
        /// <summary>
        /// Unique name of the widget.
        /// </summary>
        [NotNull]
        public string Name { get; }

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

        [CanBeNull]
        public WidgetDimensions DefaultDimensions { get; set; }

        public List<string> RequiredPermissions { get; set; }

        public List<WidgetResourceItem> Styles { get; }

        public List<WidgetResourceItem> Scripts { get; }

        public WidgetDefinition(
            [NotNull] string name,
            [NotNull] Type viewComponentType,
            [CanBeNull] ILocalizableString displayName = null)
        {
            Name = Check.NotNullOrWhiteSpace(name, nameof(name));
            ViewComponentType = Check.NotNull(viewComponentType, nameof(viewComponentType));
            DisplayName = displayName ?? new FixedLocalizableString(name);

            RequiredPermissions = new List<string>();
            Styles = new List<WidgetResourceItem>();
            Scripts = new List<WidgetResourceItem>();
        }

        public WidgetDefinition WithPermission([NotNull] string permissionName)
        {
            Check.NotNullOrWhiteSpace(permissionName, nameof(permissionName));
            RequiredPermissions.Add(permissionName);
            return this;
        }

        public WidgetDefinition WithDefaultDimensions(int width, int height)
        {
            DefaultDimensions = new WidgetDimensions(width, height);
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
}
