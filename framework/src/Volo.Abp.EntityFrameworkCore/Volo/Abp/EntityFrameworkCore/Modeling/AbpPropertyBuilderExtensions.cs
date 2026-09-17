using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volo.Abp.EntityFrameworkCore.ValueComparers;
using Volo.Abp.EntityFrameworkCore.ValueConverters;

namespace Volo.Abp.EntityFrameworkCore.Modeling;

public static class AbpPropertyBuilderExtensions
{
    /// <summary>
    /// Stores the property as a JSON serialized string with snapshot-based
    /// change tracking.
    /// </summary>
    /// <remarks>
    /// A fallback for EF Core providers without owned-JSON (ToJson) or primitive
    /// collection mapping support. The value is converted as a whole: JSON path
    /// querying and partial updates are not available, and the property type must
    /// fully round-trip with System.Text.Json.
    /// </remarks>
    public static PropertyBuilder<TProperty> HasAbpJsonConversion<TProperty>(
        this PropertyBuilder<TProperty> propertyBuilder)
    {
        return propertyBuilder.HasConversion(
            new AbpJsonValueConverter<TProperty>(),
            new AbpJsonValueComparer<TProperty>());
    }
}
