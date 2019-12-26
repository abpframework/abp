using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;

namespace Volo.Abp.EntityFrameworkCore.Modeling
{
    public static class AbpPropertyBuilderExtensions
    {
        /// <summary>
        /// Serialize and property attributes using json(JsonConvert).
        /// </summary>
        /// <param name="propertyBuilder">The builder for the property being configured.</param>
        /// <param name="valueComparer">Sets the custom ValueComparer for this property.</param>
        public static PropertyBuilder<TPropertyType> ConfigureJsonConversionWithValueComparer<TPropertyType>(
            this PropertyBuilder<TPropertyType> propertyBuilder,
            ValueComparer valueComparer)
        {
            propertyBuilder.HasConversion(
                    d => JsonConvert.SerializeObject(d, Formatting.None),
                    s => JsonConvert.DeserializeObject<TPropertyType>(s)
                )
                .Metadata.SetValueComparer(valueComparer);

            return propertyBuilder;
        }

        /// <summary>
        /// Serialize and property attributes using json(JsonConvert).
        /// </summary>
        /// <param name="propertyBuilder">The builder for the property being configured.</param>
        /// <param name="favorStructuralComparisons">
        ///     If true, then EF will use ValueComparer if the type
        ///     implements it. This is usually used when byte arrays act as keys.
        /// </param>
        public static PropertyBuilder<TPropertyType> ConfigureJsonConversionWithValueComparer<TPropertyType>(
            this PropertyBuilder<TPropertyType> propertyBuilder,
            bool favorStructuralComparisons)
        {
            propertyBuilder.HasConversion(
                    d => JsonConvert.SerializeObject(d, Formatting.None),
                    s => JsonConvert.DeserializeObject<TPropertyType>(s)
                )
                .Metadata.SetValueComparer(new ValueComparer<TPropertyType>(favorStructuralComparisons));

            return propertyBuilder;
        }

        /// <summary>
        /// Serialize and property attributes using json(JsonConvert).
        /// Creates a new ValueComparer with the given comparison expression.
        /// A shallow copy will be used for the snapshot.
        /// </summary>
        /// <param name="propertyBuilder">The builder for the property being configured.</param>
        /// <param name="equalsExpression">The comparison expression.</param>
        /// <param name="hashCodeExpression">The associated hash code generator.</param>
        public static PropertyBuilder<TPropertyType> ConfigureJsonConversionWithValueComparer<TPropertyType>(
            this PropertyBuilder<TPropertyType> propertyBuilder,
            Expression<Func<TPropertyType, TPropertyType, bool>> equalsExpression,
            Expression<Func<TPropertyType, int>> hashCodeExpression)
        {
            propertyBuilder.HasConversion(
                    d => JsonConvert.SerializeObject(d, Formatting.None),
                    s => JsonConvert.DeserializeObject<TPropertyType>(s)
                )
                .Metadata.SetValueComparer(new ValueComparer<TPropertyType>(
                    equalsExpression,
                    hashCodeExpression));

            return propertyBuilder;
        }

        /// <summary>
        /// Serialize and property attributes using json(JsonConvert).
        /// <para>
        ///     Creates a new ValueComparer with the given comparison and
        ///     snapshotting expressions.
        /// </para>
        /// <para>
        ///     Snapshotting is the process of creating a copy of the value into a snapshot so it can
        ///     later be compared to determine if it has changed. For some types, such as collections,
        ///     this needs to be a deep copy of the collection rather than just a shallow copy of the
        ///     reference.
        /// </para>
        /// </summary>
        /// <param name="propertyBuilder">The builder for the property being configured.</param>
        /// <param name="equalsExpression">The comparison expression.</param>
        /// <param name="hashCodeExpression">The associated hash code generator.</param>
        /// <param name="snapshotExpression">The snapshot expression.</param>
        public static PropertyBuilder<TPropertyType> ConfigureJsonConversionWithValueComparer<TPropertyType>(
            this PropertyBuilder<TPropertyType> propertyBuilder,
            Expression<Func<TPropertyType, TPropertyType, bool>> equalsExpression,
            Expression<Func<TPropertyType, int>> hashCodeExpression,
            Expression<Func<TPropertyType, TPropertyType>> snapshotExpression)
        {
            propertyBuilder.HasConversion(
                    d => JsonConvert.SerializeObject(d, Formatting.None),
                    s => JsonConvert.DeserializeObject<TPropertyType>(s)
                )
                .Metadata.SetValueComparer(new ValueComparer<TPropertyType>(
                    equalsExpression,
                    hashCodeExpression,
                    snapshotExpression));

            return propertyBuilder;
        }
    }
}