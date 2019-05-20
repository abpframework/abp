using System;
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

        public Type ViewComponentType { get; set; }

        public WidgetDefinition(
            [NotNull] string name,
            [NotNull] Type viewComponentType,
            [CanBeNull] ILocalizableString displayName)
        {
            Name = Check.NotNullOrWhiteSpace(name, nameof(name));
            ViewComponentType = Check.NotNull(viewComponentType, nameof(viewComponentType));
            DisplayName = displayName ?? new FixedLocalizableString(name);
        }
    }
}
