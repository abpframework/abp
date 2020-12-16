using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Volo.Abp.Localization;
using Volo.Abp.ObjectExtending.Modularity;

namespace Volo.Abp.ObjectExtending
{
    public interface IBasicObjectExtensionPropertyInfo
    {
        [NotNull]
        public string Name { get; }

        [NotNull]
        public Type Type { get; }

        [NotNull]
        public List<Attribute> Attributes { get; }

        [NotNull]
        public List<Action<ObjectExtensionPropertyValidationContext>> Validators { get; }

        [CanBeNull]
        public ILocalizableString DisplayName { get; }

        /// <summary>
        /// Uses as the default value if <see cref="DefaultValueFactory"/> was not set.
        /// </summary>
        [CanBeNull]
        public object DefaultValue { get; set; }

        /// <summary>
        /// Used with the first priority to create the default value for the property.
        /// Uses to the <see cref="DefaultValue"/> if this was not set.
        /// </summary>
        [CanBeNull]
        public Func<object> DefaultValueFactory { get; set; }
    }
}