using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp.Localization;

namespace Volo.Abp.AspNetCore.Mvc.UI.Dashboards
{
    public class GlobalFilterDefinition
    {
        /// <summary>
        /// Unique name of the Global Filter.
        /// </summary>
        [NotNull]
        public string Name { get; }

        /// <summary>
        /// Display name of the Global Filter.
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

        public GlobalFilterDefinition(
            [NotNull] string name,
            [CanBeNull] ILocalizableString displayName,
            [NotNull] Type viewComponentType)
        {
            Name = Check.NotNullOrWhiteSpace(name, nameof(name));
            DisplayName = displayName ?? new FixedLocalizableString(name);
            ViewComponentType = Check.NotNull(viewComponentType, nameof(viewComponentType));
        }
    }
}
