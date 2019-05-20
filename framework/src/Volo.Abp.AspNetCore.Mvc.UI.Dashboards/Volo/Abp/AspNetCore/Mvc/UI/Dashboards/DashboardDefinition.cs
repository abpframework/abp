using System.Collections.Generic;
using JetBrains.Annotations;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;
using Volo.Abp.Localization;

namespace Volo.Abp.AspNetCore.Mvc.UI.Dashboards
{
    public class DashboardDefinition
    {
        /// <summary>
        /// Unique name of the dashboard.
        /// </summary>
        [NotNull]
        public string Name { get; }

        /// <summary>
        /// A list of Widgets available for this dashboard.
        /// </summary>
        public List<DashboardWidgetConfiguration> AvailableWidgets { get; }

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

        public DashboardDefinition(
            [NotNull] string name,
            [CanBeNull] ILocalizableString displayName)
        {
            Name = Check.NotNullOrWhiteSpace(name, nameof(name));
            DisplayName = displayName ?? new FixedLocalizableString(name);

            AvailableWidgets = new List<DashboardWidgetConfiguration>();
        }
    }
}
