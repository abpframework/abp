using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Versioning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Volo.Abp.Reflection;

namespace Volo.Abp.AspNetCore.Mvc.Conventions;

public class ConventionalControllerSetting
{
    [NotNull]
    public Assembly Assembly { get; }

    [NotNull]
    public HashSet<Type> ControllerTypes { get; } //TODO: Internal?

    /// <summary>
    /// Set true to use the old style URL path style.
    /// Default: null (uses the value of the <see cref="AbpConventionalControllerOptions.UseV3UrlStyle"/>).
    /// </summary>
    public bool? UseV3UrlStyle { get; set; }

    [NotNull]
    public string RootPath
    {
        get => _rootPath;
        set
        {
            Check.NotNull(value, nameof(value));
            _rootPath = value;
        }
    }
    private string _rootPath;

    [NotNull]
    public string RemoteServiceName
    {
        get => _remoteServiceName;
        set
        {
            Check.NotNull(value, nameof(value));
            _remoteServiceName = value;
        }
    }
    private string _remoteServiceName;

    [CanBeNull]
    public Func<Type, bool> TypePredicate { get; set; }

    [CanBeNull]
    public Action<ControllerModel> ControllerModelConfigurer { get; set; }

    [CanBeNull]
    public Func<UrlControllerNameNormalizerContext, string> UrlControllerNameNormalizer { get; set; }

    [CanBeNull]
    public Func<UrlActionNameNormalizerContext, string> UrlActionNameNormalizer { get; set; }

    public List<ApiVersion> ApiVersions { get; }

    public Action<ApiVersioningOptions> ApiVersionConfigurer { get; set; }

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
            .WhereIf(TypePredicate != null, TypePredicate);

        foreach (var type in types)
        {
            ControllerTypes.Add(type);
        }
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
}
