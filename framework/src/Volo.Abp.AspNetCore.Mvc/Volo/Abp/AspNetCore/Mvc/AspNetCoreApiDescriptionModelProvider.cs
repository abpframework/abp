using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Volo.Abp.Application.Services;
using Volo.Abp.AspNetCore.Mvc.Conventions;
using Volo.Abp.AspNetCore.Mvc.Utils;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Modeling;
using Volo.Abp.Reflection;
using Volo.Abp.Threading;

namespace Volo.Abp.AspNetCore.Mvc
{
    public class AspNetCoreApiDescriptionModelProvider : IApiDescriptionModelProvider, ITransientDependency
    {
        public ILogger<AspNetCoreApiDescriptionModelProvider> Logger { get; set; }

        private readonly AspNetCoreApiDescriptionModelProviderOptions _options;
        private readonly IApiDescriptionGroupCollectionProvider _descriptionProvider;
        private readonly AbpAspNetCoreMvcOptions _abpAspNetCoreMvcOptions;
        private readonly AbpApiDescriptionModelOptions _modelOptions;

        public AspNetCoreApiDescriptionModelProvider(
            IOptions<AspNetCoreApiDescriptionModelProviderOptions> options,
            IApiDescriptionGroupCollectionProvider descriptionProvider,
            IOptions<AbpAspNetCoreMvcOptions> abpAspNetCoreMvcOptions,
            IOptions<AbpApiDescriptionModelOptions> modelOptions)
        {
            _options = options.Value;
            _descriptionProvider = descriptionProvider;
            _abpAspNetCoreMvcOptions = abpAspNetCoreMvcOptions.Value;
            _modelOptions = modelOptions.Value;

            Logger = NullLogger<AspNetCoreApiDescriptionModelProvider>.Instance;
        }

        public ApplicationApiDescriptionModel CreateApiModel(ApplicationApiDescriptionModelRequestDto input)
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

                    AddApiDescriptionToModel(apiDescription, model, input);
                }
            }

            return model;
        }

        private void AddApiDescriptionToModel(
            ApiDescription apiDescription,
            ApplicationApiDescriptionModel applicationModel,
            ApplicationApiDescriptionModelRequestDto input)
        {
            var controllerType = apiDescription
                .ActionDescriptor
                .AsControllerActionDescriptor()
                .ControllerTypeInfo;

            var setting = FindSetting(controllerType);

            var moduleModel = applicationModel.GetOrAddModule(
                GetRootPath(controllerType, setting),
                GetRemoteServiceName(controllerType, setting)
            );

            var controllerModel = moduleModel.GetOrAddController(
                _options.ControllerNameGenerator(controllerType, setting),
                FindGroupName(controllerType) ?? apiDescription.GroupName,
                controllerType,
                _modelOptions.IgnoredInterfaces
            );

            var method = apiDescription.ActionDescriptor.GetMethodInfo();

            var uniqueMethodName = _options.ActionNameGenerator(method);
            if (controllerModel.Actions.ContainsKey(uniqueMethodName))
            {
                Logger.LogWarning(
                    $"Controller '{controllerModel.ControllerName}' contains more than one action with name '{uniqueMethodName}' for module '{moduleModel.RootPath}'. Ignored: " +
                    method);
                return;
            }

            Logger.LogDebug($"ActionApiDescriptionModel.Create: {controllerModel.ControllerName}.{uniqueMethodName}");

            bool? allowAnonymous = null;
            if (apiDescription.ActionDescriptor.EndpointMetadata.Any(x => x is IAllowAnonymous))
            {
                allowAnonymous = true;
            }
            else if (apiDescription.ActionDescriptor.EndpointMetadata.Any(x => x is IAuthorizeData))
            {
                allowAnonymous = false;
            }

            var implementFrom = controllerType.FullName;

            var interfaceType = controllerType.GetInterfaces().FirstOrDefault(i => i.GetMethods().Any(x => x.ToString() == method.ToString()));
            if (interfaceType != null)
            {
                implementFrom = TypeHelper.GetFullNameHandlingNullableAndGenerics(interfaceType);
            }

            var actionModel = controllerModel.AddAction(
                uniqueMethodName,
                ActionApiDescriptionModel.Create(
                    uniqueMethodName,
                    method,
                    apiDescription.RelativePath,
                    apiDescription.HttpMethod,
                    GetSupportedVersions(controllerType, method, setting),
                    allowAnonymous,
                    implementFrom
                )
            );

            if (input.IncludeTypes)
            {
                AddCustomTypesToModel(applicationModel, method);
            }

            AddParameterDescriptionsToModel(actionModel, method, apiDescription);
        }

        private static List<string> GetSupportedVersions(Type controllerType, MethodInfo method,
            ConventionalControllerSetting setting)
        {
            var supportedVersions = new List<ApiVersion>();

            var mapToAttributes = method.GetCustomAttributes<MapToApiVersionAttribute>().ToArray();
            if (mapToAttributes.Any())
            {
                supportedVersions.AddRange(
                    mapToAttributes.SelectMany(a => a.Versions)
                );
            }
            else
            {
                supportedVersions.AddRange(
                    controllerType.GetCustomAttributes<ApiVersionAttribute>().SelectMany(a => a.Versions)
                );

                setting?.ApiVersions.ForEach(supportedVersions.Add);
            }

            return supportedVersions.Select(v => v.ToString()).Distinct().ToList();
        }

        private void AddCustomTypesToModel(ApplicationApiDescriptionModel applicationModel, MethodInfo method)
        {
            foreach (var parameterInfo in method.GetParameters())
            {
                AddCustomTypesToModel(applicationModel, parameterInfo.ParameterType);
            }

            AddCustomTypesToModel(applicationModel, method.ReturnType);
        }

        private static void AddCustomTypesToModel(ApplicationApiDescriptionModel applicationModel,
            [CanBeNull] Type type)
        {
            if (type == null)
            {
                return;
            }

            if (type.IsGenericParameter)
            {
                return;
            }

            type = AsyncHelper.UnwrapTask(type);

            if (type == typeof(object) ||
                type == typeof(void) ||
                type == typeof(Enum) ||
                type == typeof(ValueType) ||
                TypeHelper.IsPrimitiveExtended(type))
            {
                return;
            }

            if (TypeHelper.IsDictionary(type, out var keyType, out var valueType))
            {
                AddCustomTypesToModel(applicationModel, keyType);
                AddCustomTypesToModel(applicationModel, valueType);
                return;
            }

            if (TypeHelper.IsEnumerable(type, out var itemType))
            {
                AddCustomTypesToModel(applicationModel, itemType);
                return;
            }

            if (type.IsGenericType && !type.IsGenericTypeDefinition)
            {
                var genericTypeDefinition = type.GetGenericTypeDefinition();

                AddCustomTypesToModel(applicationModel, genericTypeDefinition);

                foreach (var genericArgument in type.GetGenericArguments())
                {
                    AddCustomTypesToModel(applicationModel, genericArgument);
                }

                return;
            }

            var typeName = CalculateTypeName(type);
            if (applicationModel.Types.ContainsKey(typeName))
            {
                return;
            }

            applicationModel.Types[typeName] = TypeApiDescriptionModel.Create(type);

            AddCustomTypesToModel(applicationModel, type.BaseType);

            foreach (var propertyInfo in type.GetProperties().Where(p => p.DeclaringType == type))
            {
                AddCustomTypesToModel(applicationModel, propertyInfo.PropertyType);
            }
        }

        private static string CalculateTypeName(Type type)
        {
            if (!type.IsGenericTypeDefinition)
            {
                return TypeHelper.GetFullNameHandlingNullableAndGenerics(type);
            }

            var i = 0;
            var argumentList = type
                .GetGenericArguments()
                .Select(_ => "T" + i++)
                .JoinAsString(",");

            return $"{type.FullName.Left(type.FullName.IndexOf('`'))}<{argumentList}>";
        }

        private void AddParameterDescriptionsToModel(ActionApiDescriptionModel actionModel, MethodInfo method,
            ApiDescription apiDescription)
        {
            if (!apiDescription.ParameterDescriptions.Any())
            {
                return;
            }

            var parameterDescriptionNames = apiDescription
                .ParameterDescriptions
                .Select(p => p.Name)
                .ToArray();

            var methodParameterNames = method
                .GetParameters()
                .Where(IsNotFromServicesParameter)
                .Select(GetMethodParamName)
                .ToArray();

            var matchedMethodParamNames = ArrayMatcher.Match(
                parameterDescriptionNames,
                methodParameterNames
            );

            for (var i = 0; i < apiDescription.ParameterDescriptions.Count; i++)
            {
                var parameterDescription = apiDescription.ParameterDescriptions[i];
                var matchedMethodParamName = matchedMethodParamNames.Length > i
                    ? matchedMethodParamNames[i]
                    : parameterDescription.Name;

                actionModel.AddParameter(ParameterApiDescriptionModel.Create(
                        parameterDescription.Name,
                        _options.ApiParameterNameGenerator?.Invoke(parameterDescription),
                        matchedMethodParamName,
                        parameterDescription.Type,
                        parameterDescription.RouteInfo?.IsOptional ?? false,
                        parameterDescription.RouteInfo?.DefaultValue,
                        parameterDescription.RouteInfo?.Constraints?.Select(c => c.GetType().Name).ToArray(),
                        parameterDescription.Source.Id,
                        parameterDescription.ModelMetadata?.ContainerType != null
                            ? parameterDescription.ParameterDescriptor?.Name ?? string.Empty
                            : string.Empty
                    )
                );
            }
        }

        private static bool IsNotFromServicesParameter(ParameterInfo parameterInfo)
        {
            return !parameterInfo.IsDefined(typeof(FromServicesAttribute), true);
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

            return modelNameProvider.Name ?? parameterInfo.Name;
        }

        private static string GetRootPath([NotNull] Type controllerType,
            [CanBeNull] ConventionalControllerSetting setting)
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

        private string GetRemoteServiceName(Type controllerType, [CanBeNull] ConventionalControllerSetting setting)
        {
            if (setting != null)
            {
                return setting.RemoteServiceName;
            }

            var remoteServiceAttr =
                controllerType.GetCustomAttributes().OfType<RemoteServiceAttribute>().FirstOrDefault();
            if (remoteServiceAttr?.Name != null)
            {
                return remoteServiceAttr.Name;
            }

            return ModuleApiDescriptionModel.DefaultRemoteServiceName;
        }

        private string FindGroupName(Type controllerType)
        {
            var controllerNameAttribute =
                controllerType.GetCustomAttributes().OfType<ControllerNameAttribute>().FirstOrDefault();

            if (controllerNameAttribute?.Name != null)
            {
                return controllerNameAttribute.Name;
            }

            return null;
        }

        [CanBeNull]
        private ConventionalControllerSetting FindSetting(Type controllerType)
        {
            foreach (var controllerSetting in _abpAspNetCoreMvcOptions.ConventionalControllers.ConventionalControllerSettings)
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
