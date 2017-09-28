using System;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Volo.Abp.AspNetCore.Mvc
{
    public class AbpControllerAssemblySetting
    {
        [NotNull]
        public Assembly Assembly { get; }

        [NotNull]
        public string RootPath { get; }

        [CanBeNull]
        public Func<Type, bool> TypePredicate { get; set; }

        [CanBeNull]
        public Action<ControllerModel> ControllerModelConfigurer { get; set; }

        [CanBeNull]
        public Func<UrlControllerNameNormalizerContext, string> UrlControllerNameNormalizer { get; set; }

        [CanBeNull]
        public Func<UrlActionNameNormalizerContext, string> UrlActionNameNormalizer { get; set; }

        public AbpControllerAssemblySetting([NotNull] Assembly assembly, [NotNull] string rootPath)
        {
            Check.NotNull(assembly, rootPath);

            Assembly = assembly;
            RootPath = rootPath;
        }
    }
}