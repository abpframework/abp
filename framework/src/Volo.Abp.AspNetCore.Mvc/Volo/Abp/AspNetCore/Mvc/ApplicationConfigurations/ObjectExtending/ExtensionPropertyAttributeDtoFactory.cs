using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Reflection;

namespace Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations.ObjectExtending
{
    public class ExtensionPropertyAttributeDtoFactory : IExtensionPropertyAttributeDtoFactory, ITransientDependency
    {
        public virtual ExtensionPropertyAttributeDto Create(Attribute attribute)
        {
            return new ExtensionPropertyAttributeDto
            {
                TypeSimple = GetSimplifiedName(attribute),
                Config = CreateConfiguration(attribute)
            };
        }

        protected virtual string GetSimplifiedName(Attribute attribute)
        {
            return attribute.GetType().Name.ToCamelCase().RemovePostFix("Attribute");
        }

        protected virtual Dictionary<string, object> CreateConfiguration(Attribute attribute)
        {
            var configuration = new Dictionary<string, object>();

            AddPropertiesToConfiguration(attribute, configuration);

            return configuration;
        }

        protected virtual void AddPropertiesToConfiguration(Attribute attribute, Dictionary<string, object> configuration)
        {
            var properties = attribute
                .GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public);

            foreach (var property in properties)
            {
                if (IgnoreProperty(attribute, property))
                {
                    continue;
                }

                var value = GetPropertyValue(attribute, property);
                if (value == null)
                {
                    continue;
                }

                configuration[property.Name.ToCamelCase()] = value;
            }
        }

        protected virtual bool IgnoreProperty(Attribute attribute, PropertyInfo property)
        {
            if (property.DeclaringType == null ||
                property.DeclaringType.IsIn(typeof(ValidationAttribute), typeof(Attribute), typeof(object)))
            {
                return true;
            }

            if (property.PropertyType == typeof(DisplayFormatAttribute))
            {
                return true;
            }

            return false;
        }

        protected virtual object GetPropertyValue(Attribute attribute, PropertyInfo property)
        {
            var value = property.GetValue(attribute);
            if (value == null)
            {
                return null;
            }

            if (property.PropertyType.IsEnum)
            {
                return Enum.GetName(property.PropertyType, value);
            }

            if (property.PropertyType == typeof(Type))
            {
                return TypeHelper.GetSimplifiedName((Type) value);
            }

            return value;
        }
    }
}
