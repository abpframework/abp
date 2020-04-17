﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;

namespace Volo.Abp.ObjectExtending
{
    public class ObjectExtensionPropertyInfo
    {
        [NotNull]
        public ObjectExtensionInfo ObjectExtension { get; }

        [NotNull]
        public string Name { get; }

        [NotNull]
        public Type Type { get; }

        [NotNull]
        public List<ValidationAttribute> ValidationAttributes { get; }

        [NotNull]
        public List<Action<ObjectExtensionPropertyValidationContext>> Validators { get; }

        /// <summary>
        /// Indicates whether to check the other side of the object mapping
        /// if it explicitly defines the property. This property is used in;
        ///
        /// * .MapExtraPropertiesTo() extension method.
        /// * .MapExtraProperties() configuration for the AutoMapper.
        ///
        /// It this is true, these methods check if the mapping object
        /// has defined the property using the <see cref="ObjectExtensionManager"/>.
        ///
        /// Default: null (unspecified, uses the default logic).
        /// </summary>
        public bool? CheckPairDefinitionOnMapping { get; set; }

        [NotNull]
        public Dictionary<object, object> Configuration { get; }

        public ObjectExtensionPropertyInfo(
            [NotNull] ObjectExtensionInfo objectExtension, 
            [NotNull] Type type, 
            [NotNull] string name)
        {
            ObjectExtension = Check.NotNull(objectExtension, nameof(objectExtension));
            Type = Check.NotNull(type, nameof(type));
            Name = Check.NotNull(name, nameof(name));

            Configuration = new Dictionary<object, object>();
            ValidationAttributes = new List<ValidationAttribute>();
            Validators = new List<Action<ObjectExtensionPropertyValidationContext>>();
        }
    }
}
