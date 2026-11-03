using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Volo.Abp.Collections;

namespace Volo.Abp.BlobStoring;

public class BlobContainerConfiguration
{
    private Type? _providerType;

    /// <summary>
    /// The provider to be used to store BLOBs of this container.
    /// </summary>
    public Type? ProviderType
    {
        get => _providerType ?? _fallbackConfiguration?.ProviderType;
        set => _providerType = value;
    }

    /// <summary>
    /// Indicates whether this container is multi-tenant or not.
    ///
    /// If this is <code>false</code> and your application is multi-tenant,
    /// then the container is shared by all tenants in the system.
    ///
    /// This can be <code>true</code> even if your application is not multi-tenant.
    ///
    /// Default: true.
    /// </summary>
    public bool IsMultiTenant { get; set; } = true;

    public ITypeList<IBlobNamingNormalizer> NamingNormalizers { get; }

    /// <summary>
    /// The <see cref="IBlobPipelineContributor"/> implementations transforming
    /// the content of this container while it is saved and read.
    /// </summary>
    public ITypeList<IBlobPipelineContributor> PipelineContributors { get; }

    /// <summary>
    /// Set false to stop inheriting the pipeline contributors of the default
    /// container configuration, so only the own <see cref="PipelineContributors"/>
    /// of this container run. Default: true.
    /// </summary>
    public bool InheritPipelineContributors { get; set; } = true;

    [NotNull] private readonly Dictionary<string, object?> _properties;

    private readonly BlobContainerConfiguration? _fallbackConfiguration;

    public BlobContainerConfiguration(BlobContainerConfiguration? fallbackConfiguration = null)
    {
        NamingNormalizers = new TypeList<IBlobNamingNormalizer>();
        PipelineContributors = new TypeList<IBlobPipelineContributor>();
        _fallbackConfiguration = fallbackConfiguration;
        _properties = new Dictionary<string, object?>();
    }

    /// <summary>
    /// Returns the naming normalizers in effect for this container, inheriting from the fallback
    /// configuration only when this container has none and does not override <see cref="ProviderType"/>.
    /// </summary>
    public IEnumerable<Type> GetEffectiveNamingNormalizers()
    {
        if (NamingNormalizers.Count == 0 && _providerType == null && _fallbackConfiguration != null)
        {
            return _fallbackConfiguration.GetEffectiveNamingNormalizers();
        }

        return NamingNormalizers;
    }

    /// <summary>
    /// Returns the pipeline contributors in effect for this container: the contributors
    /// of the fallback (default) configuration first, then the own ones (each contributor
    /// type runs once). Contributors are provider-independent content transformations,
    /// so overriding <see cref="ProviderType"/> does not reset the inherited ones; use
    /// <see cref="InheritPipelineContributors"/> to opt out of the inherited ones.
    /// </summary>
    public IEnumerable<Type> GetEffectivePipelineContributors()
    {
        if (_fallbackConfiguration == null || !InheritPipelineContributors)
        {
            return PipelineContributors.Distinct();
        }

        return _fallbackConfiguration
            .GetEffectivePipelineContributors()
            .Concat(PipelineContributors)
            .Distinct();
    }

    public T? GetConfigurationOrDefault<T>(string name, T? defaultValue = default)
    {
        return (T?)GetConfigurationOrNull(name, defaultValue);
    }

    public object? GetConfigurationOrNull(string name, object? defaultValue = null)
    {
        return _properties.GetOrDefault(name) ??
               _fallbackConfiguration?.GetConfigurationOrNull(name, defaultValue) ??
               defaultValue;
    }

    [NotNull]
    public BlobContainerConfiguration SetConfiguration([NotNull] string name, object? value)
    {
        Check.NotNullOrWhiteSpace(name, nameof(name));
        Check.NotNull(value, nameof(value));

        _properties[name] = value;

        return this;
    }

    [NotNull]
    public BlobContainerConfiguration ClearConfiguration([NotNull] string name)
    {
        Check.NotNullOrWhiteSpace(name, nameof(name));

        _properties.Remove(name);

        return this;
    }
}
