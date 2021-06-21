using System;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http;
using Volo.Abp.Reflection;

namespace Volo.Abp.AspNetCore.Mvc.Conventions
{
    public class ConventionalRouteBuilder : IConventionalRouteBuilder, ITransientDependency
    {
        protected AbpConventionalControllerOptions Options { get; }

        public ConventionalRouteBuilder(IOptions<AbpConventionalControllerOptions> options)
        {
            Options = options.Value;
        }

        public virtual string Build(
            string rootPath,
            string controllerName,
            ActionModel action,
            string httpMethod,
            [CanBeNull] ConventionalControllerSetting configuration)
        {
            var controllerNameInUrl = NormalizeUrlControllerName(rootPath, controllerName, action, httpMethod, configuration);

            var url = $"api/{rootPath}/{NormalizeControllerNameCase(controllerNameInUrl, configuration)}";

            //Add {id} path if needed
            var idParameterModel = action.Parameters.FirstOrDefault(p => p.ParameterName == "id");
            if (idParameterModel != null)
            {
                if (TypeHelper.IsPrimitiveExtended(idParameterModel.ParameterType, includeEnums: true))
                {
                    url += "/{id}";
                }
                else
                {
                    var properties = idParameterModel
                        .ParameterType
                        .GetProperties(BindingFlags.Instance | BindingFlags.Public);

                    foreach (var property in properties)
                    {
                        url += "/{" + NormalizeIdPropertyNameCase(property, configuration) + "}";
                    }
                }
            }

            //Add action name if needed
            var actionNameInUrl = NormalizeUrlActionName(rootPath, controllerName, action, httpMethod, configuration);
            if (!actionNameInUrl.IsNullOrEmpty())
            {
                url += $"/{NormalizeActionNameCase(actionNameInUrl, configuration)}";

                //Add secondary Id
                var secondaryIds = action.Parameters
                    .Where(p => p.ParameterName.EndsWith("Id", StringComparison.Ordinal)).ToList();
                if (secondaryIds.Count == 1)
                {
                    url += $"/{{{NormalizeSecondaryIdNameCase(secondaryIds[0], configuration)}}}";
                }
            }

            return url;
        }

        protected virtual string NormalizeUrlActionName(string rootPath, string controllerName, ActionModel action,
            string httpMethod, [CanBeNull] ConventionalControllerSetting configuration)
        {
            var actionNameInUrl = HttpMethodHelper
                .RemoveHttpMethodPrefix(action.ActionName, httpMethod)
                .RemovePostFix("Async");

            if (configuration?.UrlActionNameNormalizer == null)
            {
                return actionNameInUrl;
            }

            return configuration.UrlActionNameNormalizer(
                new UrlActionNameNormalizerContext(
                    rootPath,
                    controllerName,
                    action,
                    actionNameInUrl,
                    httpMethod
                )
            );
        }

        protected virtual string NormalizeUrlControllerName(string rootPath, string controllerName, ActionModel action,
            string httpMethod, [CanBeNull] ConventionalControllerSetting configuration)
        {
            if (configuration?.UrlControllerNameNormalizer == null)
            {
                return controllerName;
            }

            return configuration.UrlControllerNameNormalizer(
                new UrlControllerNameNormalizerContext(
                    rootPath,
                    controllerName
                )
            );
        }

        protected virtual string NormalizeControllerNameCase(string controllerName, [CanBeNull] ConventionalControllerSetting configuration)
        {
            if (configuration?.UseV3UrlStyle ?? Options.UseV3UrlStyle)
            {
                return controllerName.ToCamelCase();
            }
            else
            {
                return controllerName.ToKebabCase();
            }
        }

        protected virtual string NormalizeActionNameCase(string actionName, [CanBeNull] ConventionalControllerSetting configuration)
        {
            if (configuration?.UseV3UrlStyle ?? Options.UseV3UrlStyle)
            {
                return actionName.ToCamelCase();
            }
            else
            {
                return actionName.ToKebabCase();
            }
        }

        protected virtual string NormalizeIdPropertyNameCase(PropertyInfo property, [CanBeNull] ConventionalControllerSetting configuration)
        {
            return property.Name;
        }

        protected virtual string NormalizeSecondaryIdNameCase(ParameterModel secondaryId, [CanBeNull] ConventionalControllerSetting configuration)
        {
            return secondaryId.ParameterName;
        }
    }
}
