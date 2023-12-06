using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using Asp.Versioning;
using Volo.Abp.Reflection;

namespace Volo.Abp.AspNetCore.Mvc.Conventions;

public class ConventionalControllerSetting
{
    [NotNull]
    public Assembly Assembly { get; }

    [NotNull]
    internal HashSet<Type> ControllerTypes { get; }

    /// <summary>
    /// Set true to use the old style URL path style.
    /// Default: null (uses the value of the <see cref="AbpConventionalControllerOptions.UseV3UrlStyle"/>).
    /// </summary>
    public bool? UseV3UrlStyle { get; set; }

    [NotNull]
    public string RootPath {
        get => _rootPath;
        set {
            Check.NotNull(value, nameof(value));
            _rootPath = value;
        }
    }
    private string _rootPath = default!;

    [NotNull]
    public string RemoteServiceName {
        get => _remoteServiceName;
        set {
            Check.NotNull(value, nameof(value));
            _remoteServiceName = value;
        }
    }
    private string _remoteServiceName = default!;

    public Func<Type, bool>? TypePredicate { get; set; }

    /// <summary>
    /// Default value: All.
    /// </summary>
    public ApplicationServiceTypes ApplicationServiceTypes { get; set; } = ApplicationServiceTypes.All;

    public Action<ControllerModel>? ControllerModelConfigurer { get; set; }

    public Func<UrlControllerNameNormalizerContext, string>? UrlControllerNameNormalizer { get; set; }

    public Func<UrlActionNameNormalizerContext, string>? UrlActionNameNormalizer { get; set; }

    public List<ApiVersion> ApiVersions { get; }

    public Action<MvcApiVersioningOptions>? MvcApiVersioningConfigurer { get; set; }

    public ConventionalControllerSetting(
        [NotNull] Assembly assembly,
        [NotNull] string rootPath,
        [NotNull] string remoteServiceName)
    {
        Assembly = Check.NotNull(assembly, nameof(assembly));
        RootPath = Check.NotNull(rootPath, nameof(rootPath));
        RemoteServiceName = Check.NotNull(remoteServiceName, nameof(remoteServiceName));

        ControllerTypes = new HashSet<Type>();
        ApiVersions = new List<ApiVersion>();
    }

    public void Initialize()
    {
        var types = Assembly.GetTypes()
            .Where(IsRemoteService)
            .Where(IsPreferredApplicationServiceType)
            .WhereIf(TypePredicate != null, TypePredicate!);

        foreach (var type in types)
        {
            ControllerTypes.Add(type);
        }
    }

    public IReadOnlyList<Type> GetControllerTypes()
    {
        return ControllerTypes.ToImmutableList();
    }

    private static bool IsRemoteService(Type type)
    {
        if (!type.IsPublic || type.IsAbstract || type.IsGenericType)
        {
            return false;
        }

        var remoteServiceAttr = ReflectionHelper.GetSingleAttributeOrDefault<RemoteServiceAttribute>(type);
        if (remoteServiceAttr != null && !remoteServiceAttr.IsEnabledFor(type))
        {
            return false;
        }

        if (typeof(IRemoteService).IsAssignableFrom(type))
        {
            return true;
        }

        return false;
    }

    private bool IsPreferredApplicationServiceType(Type type)
    {
        if (ApplicationServiceTypes == ApplicationServiceTypes.ApplicationServices)
        {
            return !IntegrationServiceAttribute.IsDefinedOrInherited(type);
        }

        if (ApplicationServiceTypes == ApplicationServiceTypes.IntegrationServices)
        {
            return IntegrationServiceAttribute.IsDefinedOrInherited(type);
        }

        return true;
    }
}
