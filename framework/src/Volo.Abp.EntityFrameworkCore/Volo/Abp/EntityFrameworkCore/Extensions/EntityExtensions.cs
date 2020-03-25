using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Volo.Abp.EntityFrameworkCore.Extensions
{
    public class EntityExtensionInfo
    {
        public Dictionary<string, PropertyExtensionInfo> Properties { get; set; }

        public EntityExtensionInfo()
        {
            Properties = new Dictionary<string, PropertyExtensionInfo>();
        }
    }

    public class PropertyExtensionInfo
    {
        public List<Action<PropertyBuilder>> Actions { get; }

        public Type PropertyType { get; }

        public PropertyExtensionInfo(Type propertyType)
        {
            PropertyType = propertyType;
            Actions = new List<Action<PropertyBuilder>>();
        }
    }

    public static class EntityExtensions
    {
        private static readonly Dictionary<Type, EntityExtensionInfo> ExtensionInfos;

        //TODO: Use PropertyBuilder<TProperty> instead

        static EntityExtensions()
        {
            ExtensionInfos = new Dictionary<Type, EntityExtensionInfo>();
        }

        public static void AddProperty<TEntity>(
            string name,
            Type propertyType,
            Action<PropertyBuilder> propertyBuildAction)
        {
            var extensionInfo = ExtensionInfos
                .GetOrAdd(typeof(TEntity), () => new EntityExtensionInfo());

            var propertyExtensionInfo = extensionInfo.Properties
                .GetOrAdd(name, () => new PropertyExtensionInfo(propertyType));

            propertyExtensionInfo.Actions.Add(propertyBuildAction);
        }

        public static void ConfigureProperties<TEntity>(EntityTypeBuilder entityTypeBuilder)
        {
            var entityExtensionInfo = ExtensionInfos.GetOrDefault(typeof(TEntity));
            if (entityExtensionInfo == null)
            {
                return;
            }

            foreach (var propertyExtensionInfo in entityExtensionInfo.Properties)
            {
                var property = entityTypeBuilder.Property(propertyExtensionInfo.Value.PropertyType, propertyExtensionInfo.Key);
                foreach (var action in propertyExtensionInfo.Value.Actions)
                {
                    action(property);
                }
            }
        }

        public static string[] GetPropertyNames(Type entityType)
        {
            var entityExtensionInfo = ExtensionInfos.GetOrDefault(entityType);
            if (entityExtensionInfo == null)
            {
                return Array.Empty<string>();
            }

            return entityExtensionInfo
                .Properties
                .Select(p => p.Key)
                .ToArray();
        }
    }
}
