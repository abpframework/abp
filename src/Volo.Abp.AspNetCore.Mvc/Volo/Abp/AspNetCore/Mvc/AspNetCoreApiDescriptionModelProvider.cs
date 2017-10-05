using System;
using System.Linq;
using System.Reflection;
using System.Text;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Volo.Abp.Application.Services;
using Volo.Abp.AspNetCore.Mvc.Utils;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Modeling;

namespace Volo.Abp.AspNetCore.Mvc
{
    public class AspNetCoreApiDescriptionModelProvider : IApiDescriptionModelProvider, ISingletonDependency
    {
        public ILogger<AspNetCoreApiDescriptionModelProvider> Logger { get; set; }

        private readonly IApiDescriptionGroupCollectionProvider _descriptionProvider;
        private readonly AbpAspNetCoreMvcOptions _options;
        private readonly ApiDescriptionModelOptions _modelOptions;

        public AspNetCoreApiDescriptionModelProvider(
            IApiDescriptionGroupCollectionProvider descriptionProvider,
            IOptions<AbpAspNetCoreMvcOptions> options,
            IOptions<ApiDescriptionModelOptions> modelOptions)
        {
            _descriptionProvider = descriptionProvider;
            _options = options.Value;
            _modelOptions = modelOptions.Value;

            Logger = NullLogger<AspNetCoreApiDescriptionModelProvider>.Instance;
        }

        public ApplicationApiDescriptionModel CreateApiModel()
        {
            //TODO: Can cache the model?

            var model = ApplicationApiDescriptionModel.Create();

            foreach (var descriptionGroupItem in _descriptionProvider.ApiDescriptionGroups.Items)
            {
                foreach (var apiDescription in descriptionGroupItem.Items)
                {
                    if (!apiDescription.ActionDescriptor.IsControllerAction())
                    {
                        continue;
                    }

                    AddApiDescriptionToModel(apiDescription, model);
                }
            }

            return model;
        }

        private void AddApiDescriptionToModel(ApiDescription apiDescription, ApplicationApiDescriptionModel model)
        {
            var controllerType = apiDescription.ActionDescriptor.AsControllerActionDescriptor().ControllerTypeInfo.AsType();
            var setting = FindSetting(controllerType);

            var moduleModel = model.GetOrAddModule(GetRootPath(controllerType, setting));

            var controllerModel = moduleModel.GetOrAddController(controllerType.FullName, CalculateControllerName(controllerType, setting), controllerType, _modelOptions.IgnoredInterfaces);

            var method = apiDescription.ActionDescriptor.GetMethodInfo();

            var uniqueMethodName = GetUniqueActionName(method);
            if (controllerModel.Actions.ContainsKey(uniqueMethodName))
            {
                Logger.LogWarning($"Controller '{controllerModel.ControllerName}' contains more than one action with name '{uniqueMethodName}' for module '{moduleModel.RootPath}'. Ignored: " + method);
                return;
            }

            var actionModel = controllerModel.AddAction(uniqueMethodName, ActionApiDescriptionModel.Create(
                method,
                apiDescription.RelativePath,
                apiDescription.HttpMethod
            ));

            AddParameterDescriptionsToModel(actionModel, method, apiDescription);
        }

        private static string CalculateControllerName(Type controllerType, AbpControllerAssemblySetting setting)
        {
            var controllerName = controllerType.Name.RemovePostFix("Controller").RemovePostFix(ApplicationService.CommonPostfixes);

            if (setting?.UrlControllerNameNormalizer != null)
            {
                controllerName = setting.UrlControllerNameNormalizer(new UrlControllerNameNormalizerContext(setting.RootPath, controllerName));
            }

            return controllerName;
        }

        private static string GetUniqueActionName(MethodInfo method)
        {
            var methodNameBuilder = new StringBuilder(method.Name);

            var parameters = method.GetParameters();
            if (parameters.Any())
            {
                methodNameBuilder.Append("By");

                for (var i = 0; i < parameters.Length; i++)
                {
                    if (i > 0)
                    {
                        methodNameBuilder.Append("And");
                    }

                    methodNameBuilder.Append(parameters[i].Name.ToPascalCase());
                }
            }

            return methodNameBuilder.ToString();
        }

        private void AddParameterDescriptionsToModel(ActionApiDescriptionModel actionModel, MethodInfo method, ApiDescription apiDescription)
        {
            if (!apiDescription.ParameterDescriptions.Any())
            {
                return;
            }

            var matchedMethodParamNames = ArrayMatcher.Match(
                apiDescription.ParameterDescriptions.Select(p => p.Name).ToArray(),
                method.GetParameters().Select(GetMethodParamName).ToArray()
            );

            for (var i = 0; i < apiDescription.ParameterDescriptions.Count; i++)
            {
                var parameterDescription = apiDescription.ParameterDescriptions[i];
                var matchedMethodParamName = matchedMethodParamNames.Length > i
                                                 ? matchedMethodParamNames[i]
                                                 : parameterDescription.Name;

                actionModel.AddParameter(ParameterApiDescriptionModel.Create(
                        parameterDescription.Name,
                        matchedMethodParamName,
                        parameterDescription.Type,
                        parameterDescription.RouteInfo?.IsOptional ?? false,
                        parameterDescription.RouteInfo?.DefaultValue,
                        parameterDescription.RouteInfo?.Constraints?.Select(c => c.GetType().Name).ToArray(),
                        parameterDescription.Source.Id
                    )
                );
            }
        }

        public string GetMethodParamName(ParameterInfo parameterInfo)
        {
            var modelNameProvider = parameterInfo.GetCustomAttributes()
                .OfType<IModelNameProvider>()
                .FirstOrDefault();

            if (modelNameProvider == null)
            {
                return parameterInfo.Name;
            }

            return modelNameProvider.Name;
        }

        private static string GetRootPath([NotNull] Type controllerType, [CanBeNull] AbpControllerAssemblySetting setting)
        {
            if (setting != null)
            {
                return setting.RootPath;
            }

            var areaAttr = controllerType.GetCustomAttributes().OfType<AreaAttribute>().FirstOrDefault();
            if (areaAttr != null)
            {
                return areaAttr.RouteValue;
            }

            return ModuleApiDescriptionModel.DefaultRootPath;
        }

        [CanBeNull]
        private AbpControllerAssemblySetting FindSetting(Type controllerType)
        {
            foreach (var controllerSetting in _options.AppServiceControllers.ControllerAssemblySettings)
            {
                if (controllerSetting.ControllerTypes.Contains(controllerType))
                {
                    return controllerSetting;
                }
            }

            return null;
        }
    }
}
