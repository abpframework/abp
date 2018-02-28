using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Versioning;
using Volo.Abp.Application.Services;
using Volo.Abp.Reflection;

namespace Volo.Abp.AspNetCore.Mvc.Conventions
{
    public class ConventionalControllerSetting
    {
        [NotNull]
        public Assembly Assembly { get; }

        [NotNull]
        public HashSet<Type> ControllerTypes { get; }

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
        
        public ConventionalControllerSetting([NotNull] Assembly assembly, [NotNull] string rootPath)
        {
            Check.NotNull(assembly, rootPath);

            Assembly = assembly;
            RootPath = rootPath;

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
}