using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volo.Abp.Data;

namespace Volo.Abp.EntityFrameworkCore.Extensions
{
    public static class EntityExtensionManager
    {
        private static readonly Dictionary<Type, EntityExtensionInfo> ExtensionInfos;

        static EntityExtensionManager()
        {
            ExtensionInfos = new Dictionary<Type, EntityExtensionInfo>();
        }

        /// <summary>
        /// Adds an extension property for an entity.
        /// If it is already added, replaces the <paramref name="propertyBuildAction"/>
        /// by the given one!
        /// </summary>
        /// <typeparam name="TEntity">Type of the entity</typeparam>
        /// <typeparam name="TProperty">Type of the new property</typeparam>
        /// <param name="propertyName">Name of the property</param>
        /// <param name="propertyBuildAction">An action to configure the database mapping for the new property</param>
        public static void AddProperty<TEntity, TProperty>(
            [NotNull]string propertyName,
            [NotNull]Action<PropertyBuilder> propertyBuildAction)
        {
            AddProperty(
                typeof(TEntity),
                typeof(TProperty),
                propertyName,
                propertyBuildAction
            );
        }

        /// <summary>
        /// Adds an extension property for an entity.
        /// If it is already added, replaces the <paramref name="propertyBuildAction"/>
        /// by the given one!
        /// </summary>
        /// <param name="entityType">Type of the entity</param>
        /// <param name="propertyType">Type of the new property</param>
        /// <param name="propertyName">Name of the property</param>
        /// <param name="propertyBuildAction">An action to configure the database mapping for the new property</param>
        public static void AddProperty(
            Type entityType,
            Type propertyType,
            [NotNull]string propertyName,
            [NotNull]Action<PropertyBuilder> propertyBuildAction)
        {
            Check.NotNull(entityType, nameof(entityType));
            Check.NotNull(propertyType, nameof(propertyType));
            Check.NotNullOrWhiteSpace(propertyName, nameof(propertyName));
            Check.NotNull(propertyBuildAction, nameof(propertyBuildAction));

            var extensionInfo = ExtensionInfos
                .GetOrAdd(entityType, () => new EntityExtensionInfo());

            var propertyExtensionInfo = extensionInfo.Properties
                .GetOrAdd(propertyName, () => new PropertyExtensionInfo(propertyType));

            propertyExtensionInfo.Action = propertyBuildAction;
        }

        /// <summary>
        /// Configures the entity mapping for the defined extensions.
        /// </summary>
        /// <typeparam name="TEntity">The entity tye</typeparam>
        /// <param name="entityTypeBuilder">Entity type builder</param>
        public static void ConfigureExtensions<TEntity>(
            [NotNull] this EntityTypeBuilder<TEntity> entityTypeBuilder)
            where TEntity : class, IHasExtraProperties
        {
            ConfigureExtensions(typeof(TEntity), entityTypeBuilder);
        }

        /// <summary>
        /// Configures the entity mapping for the defined extensions.
        /// </summary>
        /// <param name="entityType">Type of the entity</param>
        /// <param name="entityTypeBuilder">Entity type builder</param>
        public static void ConfigureExtensions(
            [NotNull] Type entityType,
            [NotNull] EntityTypeBuilder entityTypeBuilder)
        {
            Check.NotNull(entityType, nameof(entityType));
            Check.NotNull(entityTypeBuilder, nameof(entityTypeBuilder));

            var entityExtensionInfo = ExtensionInfos.GetOrDefault(entityType);
            if (entityExtensionInfo == null)
            {
                return;
            }

            foreach (var propertyExtensionInfo in entityExtensionInfo.Properties)
            {
                var propertyName = propertyExtensionInfo.Key;
                var propertyType = propertyExtensionInfo.Value.PropertyType;

                /* Prevent multiple calls to the entityTypeBuilder.Property(...) method */
                if (entityTypeBuilder.Metadata.FindProperty(propertyName) != null)
                {
                    continue;
                }

                var property = entityTypeBuilder.Property(
                    propertyType,
                    propertyName
                );

                propertyExtensionInfo.Value.Action(property);
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
