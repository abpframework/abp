using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Volo.Abp.SimpleStateChecking;

namespace Volo.Abp.UI.Navigation
{
    public class ApplicationMenuItem : IHasMenuItems, IHasSimpleStateCheckers<ApplicationMenuItem>
    {
        private string _displayName;

        /// <summary>
        /// Default <see cref="Order"/> value of a menu item.
        /// </summary>
        public const int DefaultOrder = 1000;

        /// <summary>
        /// Unique name of the menu in the application.
        /// </summary>
        [NotNull]
        public string Name { get; }

        /// <summary>
        /// Display name of the menu item.
        /// </summary>
        [NotNull]
        public string DisplayName
        {
            get { return _displayName; }
            set
            {
                Check.NotNullOrWhiteSpace(value, nameof(value));
                _displayName = value;
            }
        }

        /// <summary>
        /// The Display order of the menu.
        /// Default value: 1000.
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// The URL to navigate when this menu item is selected.
        /// </summary>
        [CanBeNull]
        public string Url { get; set; }

        /// <summary>
        /// Icon of the menu item if exists.
        /// </summary>
        [CanBeNull]
        public string Icon { get; set; }

        /// <summary>
        /// Returns true if this menu item has no child <see cref="Items"/>.
        /// </summary>
        public bool IsLeaf => Items.IsNullOrEmpty();

        /// <summary>
        /// Target of the menu item. Can be null, "_blank", "_self", "_parent", "_top" or a frame name for web applications.
        /// </summary>
        [CanBeNull]
        public string Target { get; set; }

        /// <summary>
        /// Can be used to disable this menu item.
        /// </summary>
        public bool IsDisabled { get; set; }

        /// <inheritdoc cref="IHasMenuItems.Items"/>
        [NotNull]
        public ApplicationMenuItemList Items { get; }

        [CanBeNull]
        [Obsolete("Use RequirePermissions extension method.")]
        public string RequiredPermissionName { get; set; }

        public List<ISimpleStateChecker<ApplicationMenuItem>> SimpleStateCheckers { get; }

        /// <summary>
        /// Can be used to store a custom object related to this menu item. Optional.
        /// </summary>
        public object CustomData { get; set; }

        /// <summary>
        /// Can be used to render the element with a specific Id for DOM selections.
        /// </summary>
        public string ElementId { get; set; }

        /// <summary>
        /// Can be used to render the element with extra CSS classes.
        /// </summary>
        public string CssClass { get; set; }

        public ApplicationMenuItem(
            [NotNull] string name,
            [NotNull] string displayName,
            string url = null,
            string icon = null,
            int order = DefaultOrder,
            object customData = null,
            string target = null,
            string elementId = null,
            string cssClass = null,
            string requiredPermissionName = null)
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));
            Check.NotNullOrWhiteSpace(displayName, nameof(displayName));

            Name = name;
            DisplayName = displayName;
            Url = url;
            Icon = icon;
            Order = order;
            CustomData = customData;
            Target = target;
            ElementId = elementId ?? GetDefaultElementId();
            CssClass = cssClass;
            RequiredPermissionName = requiredPermissionName;
            SimpleStateCheckers = new List<ISimpleStateChecker<ApplicationMenuItem>>();
            Items = new ApplicationMenuItemList();
        }

        /// <summary>
        /// Adds a <see cref="ApplicationMenuItem"/> to <see cref="Items"/>.
        /// </summary>
        /// <param name="menuItem"><see cref="ApplicationMenuItem"/> to be added</param>
        /// <returns>This <see cref="ApplicationMenuItem"/> object</returns>
        public ApplicationMenuItem AddItem([NotNull] ApplicationMenuItem menuItem)
        {
            Items.Add(menuItem);
            return this;
        }

        private string GetDefaultElementId()
        {
            return "MenuItem_" + Name;
        }

        public override string ToString()
        {
            return $"[ApplicationMenuItem] Name = {Name}";
        }
    }
}
