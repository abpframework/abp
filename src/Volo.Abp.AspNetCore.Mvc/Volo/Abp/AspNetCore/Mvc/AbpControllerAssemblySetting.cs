using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Volo.Abp.Application.Services;
using Volo.Abp.Reflection;

namespace Volo.Abp.AspNetCore.Mvc
{
    public class AbpControllerAssemblySetting
    {
        [NotNull]
        public Assembly Assembly { get; }

        [NotNull]
        public List<Type> ControllerTypes { get; }

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

        public ApiVersion ApiVersion { get; set; }

        public AbpControllerAssemblySetting([NotNull] Assembly assembly, [NotNull] string rootPath)
        {
            Check.NotNull(assembly, rootPath);

            Assembly = assembly;
            RootPath = rootPath;

            ControllerTypes = new List<Type>();

            ApiVersion = new ApiVersion(1, 0);
        }

        public void Initialize()
        {
            ControllerTypes.AddRange(
                Assembly.GetTypes()
                    .Where(IsRemoteService)
                    .WhereIf(TypePredicate != null, TypePredicate)
            );
        }

        private static bool IsRemoteService(Type type)
        {
            if (!typeof(IRemoteService).IsAssignableFrom(type) || !type.IsPublic || type.IsAbstract || type.IsGenericType)
            {
                return false;
            }

            var remoteServiceAttr = ReflectionHelper.GetSingleAttributeOrDefault<RemoteServiceAttribute>(type);

            if (remoteServiceAttr != null && !remoteServiceAttr.IsEnabledFor(type))
            {
                return false;
            }

            return true;
        }
    }
}