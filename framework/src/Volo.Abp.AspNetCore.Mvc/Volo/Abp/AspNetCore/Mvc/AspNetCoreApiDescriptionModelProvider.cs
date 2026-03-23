using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Asp.Versioning;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Mvc.ApiExploring;
using Volo.Abp.AspNetCore.Mvc.Conventions;
using Volo.Abp.AspNetCore.Mvc.Utils;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Modeling;
using Volo.Abp.Reflection;
using Volo.Abp.Threading;

namespace Volo.Abp.AspNetCore.Mvc;

public class AspNetCoreApiDescriptionModelProvider : IApiDescriptionModelProvider, ITransientDependency
{
    public ILogger<AspNetCoreApiDescriptionModelProvider> Logger { get; set; }

    private readonly AspNetCoreApiDescriptionModelProviderOptions _options;
    private readonly IApiDescriptionGroupCollectionProvider _descriptionProvider;
    private readonly AbpAspNetCoreMvcOptions _abpAspNetCoreMvcOptions;
    private readonly AbpApiDescriptionModelOptions _modelOptions;
    private readonly IXmlDocumentationProvider _xmlDocProvider;

    public AspNetCoreApiDescriptionModelProvider(
        IOptions<AspNetCoreApiDescriptionModelProviderOptions> options,
        IApiDescriptionGroupCollectionProvider descriptionProvider,
        IOptions<AbpAspNetCoreMvcOptions> abpAspNetCoreMvcOptions,
        IOptions<AbpApiDescriptionModelOptions> modelOptions,
        IXmlDocumentationProvider xmlDocProvider)
    {
        _options = options.Value;
        _descriptionProvider = descriptionProvider;
        _abpAspNetCoreMvcOptions = abpAspNetCoreMvcOptions.Value;
        _modelOptions = modelOptions.Value;
        _xmlDocProvider = xmlDocProvider;

        Logger = NullLogger<AspNetCoreApiDescriptionModelProvider>.Instance;
    }

    public virtual async Task<ApplicationApiDescriptionModel> CreateApiModelAsync(ApplicationApiDescriptionModelRequestDto input)
    {
        //TODO: Can cache the model?

        var model = ApplicationApiDescriptionModel.Create();
        var populatedControllers = new HashSet<ControllerApiDescriptionModel>();

        foreach (var descriptionGroupItem in _descriptionProvider.ApiDescriptionGroups.Items)
        {
            foreach (var apiDescription in descriptionGroupItem.Items)
            {
                if (!apiDescription.ActionDescriptor.IsControllerAction())
                {
                    continue;
                }

                await AddApiDescriptionToModelAsync(apiDescription, model, input, populatedControllers);
            }
        }

        foreach (var (_, module) in model.Modules)
        {
            var controllers = module.Controllers.GroupBy(x => x.Value.Type).ToList();
            foreach (var controller in controllers.Where(x => x.Count() > 1))
            {
                var removedController = module.Controllers.RemoveAll(x => x.Value.IsRemoteService && controller.OrderBy(c => c.Value.ControllerGroupName).Skip(1).Contains(x));
                foreach (var removed in removedController)
                {
                    Logger.LogInformation($"The controller named '{removed.Value.Type}' was removed from ApplicationApiDescriptionModel because it same with other controller.");
                }
            }
        }

        model.NormalizeOrder();
        return model;
    }

    private async Task AddApiDescriptionToModelAsync(
        ApiDescription apiDescription,
        ApplicationApiDescriptionModel applicationModel,
        ApplicationApiDescriptionModelRequestDto input,
        HashSet<ControllerApiDescriptionModel> populatedControllers)
    {
        var controllerType = apiDescription
            .ActionDescriptor
            .AsControllerActionDescriptor()
            .ControllerTypeInfo;

        var setting = FindSetting(controllerType);

        var moduleModel = applicationModel.GetOrAddModule(
            GetRootPath(controllerType, apiDescription.ActionDescriptor, setting),
            GetRemoteServiceName(controllerType, setting)
        );

        var controllerModel = moduleModel.GetOrAddController(
            _options.ControllerNameGenerator(controllerType, setting),
            FindGroupName(controllerType) ?? apiDescription.GroupName,
            apiDescription.IsRemoteService(),
            apiDescription.IsIntegrationService(),
            apiDescription.GetProperty<ApiVersion>()?.ToString(),
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
        var authorizeModels = new List<AuthorizeDataApiDescriptionModel>();
        if (apiDescription.ActionDescriptor.EndpointMetadata.Any(x => x is IAllowAnonymous))
        {
            allowAnonymous = true;
        }
        else if (apiDescription.ActionDescriptor.EndpointMetadata.Any(x => x is IAuthorizeData))
        {
            allowAnonymous = false;
            var authorizeDatas = apiDescription.ActionDescriptor.EndpointMetadata.Where(x => x is IAuthorizeData).Cast<IAuthorizeData>().ToList();
            authorizeModels.AddRange(authorizeDatas.Select(authorizeData => new AuthorizeDataApiDescriptionModel
            {
                Policy = authorizeData.Policy,
                Roles = authorizeData.Roles
            }));
        }

        var implementFrom = controllerType.FullName;

        foreach (var iface in controllerType.GetInterfaces())
        {
            try
            {
                var map = controllerType.GetInterfaceMap(iface);
                if (Array.IndexOf(map.TargetMethods, method) >= 0)
                {
                    implementFrom = TypeHelper.GetFullNameHandlingNullableAndGenerics(iface);
                    break;
                }
            }
            catch (ArgumentException)
            {
                // GetInterfaceMap is not supported for some generic interface edge cases
            }
        }

        var actionModel = controllerModel.AddAction(
            uniqueMethodName,
            ActionApiDescriptionModel.Create(
                uniqueMethodName,
                method,
                apiDescription.RelativePath!,
                apiDescription.HttpMethod,
                GetSupportedVersions(controllerType, method, setting),
                allowAnonymous,
                authorizeModels,
                implementFrom
            )
        );

        if (input.IncludeTypes)
        {
            await AddCustomTypesToModelAsync(applicationModel, method, input.IncludeDescriptions);
        }

        AddParameterDescriptionsToModel(actionModel, method, apiDescription);

        if (input.IncludeDescriptions)
        {
            if (populatedControllers.Add(controllerModel))
            {
                await PopulateControllerDescriptionsAsync(controllerModel, controllerType);
            }

            var interfaceMethod = GetInterfaceMethod(method);
            await PopulateActionDescriptionsAsync(actionModel, method, interfaceMethod);
            await PopulateParameterDescriptionsAsync(actionModel, method, interfaceMethod);
        }
    }

    private static List<string> GetSupportedVersions(Type controllerType, MethodInfo method,
        ConventionalControllerSetting? setting)
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

    private async Task AddCustomTypesToModelAsync(ApplicationApiDescriptionModel applicationModel, MethodInfo method, bool includeDescriptions)
    {
        foreach (var parameterInfo in method.GetParameters())
        {
            await AddCustomTypesToModelAsync(applicationModel, parameterInfo.ParameterType, includeDescriptions);
        }

        await AddCustomTypesToModelAsync(applicationModel, method.ReturnType, includeDescriptions);
    }

    private async Task AddCustomTypesToModelAsync(ApplicationApiDescriptionModel applicationModel,
        Type? type, bool includeDescriptions)
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
            type == typeof(DateOnly) ||
            type == typeof(TimeOnly) ||
            TypeHelper.IsPrimitiveExtended(type))
        {
            return;
        }

        if (TypeHelper.IsDictionary(type, out var keyType, out var valueType))
        {
            await AddCustomTypesToModelAsync(applicationModel, keyType, includeDescriptions);
            await AddCustomTypesToModelAsync(applicationModel, valueType, includeDescriptions);
            return;
        }

        if (TypeHelper.IsEnumerable(type, out var itemType))
        {
            await AddCustomTypesToModelAsync(applicationModel, itemType, includeDescriptions);
            return;
        }

        if (type.IsGenericType && !type.IsGenericTypeDefinition)
        {
            var genericTypeDefinition = type.GetGenericTypeDefinition();

            await AddCustomTypesToModelAsync(applicationModel, genericTypeDefinition, includeDescriptions);

            foreach (var genericArgument in type.GetGenericArguments())
            {
                await AddCustomTypesToModelAsync(applicationModel, genericArgument, includeDescriptions);
            }

            return;
        }

        var typeName = CalculateTypeName(type);
        if (applicationModel.Types.ContainsKey(typeName))
        {
            return;
        }

        applicationModel.Types[typeName] = TypeApiDescriptionModel.Create(type);

        if (includeDescriptions)
        {
            await PopulateTypeDescriptionsAsync(applicationModel.Types[typeName], type);
        }

        await AddCustomTypesToModelAsync(applicationModel, type.BaseType, includeDescriptions);

        foreach (var propertyInfo in type.GetProperties().Where(p => p.DeclaringType == type))
        {
            await AddCustomTypesToModelAsync(applicationModel, propertyInfo.PropertyType, includeDescriptions);
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

        return $"{type.FullName!.Left(type.FullName!.IndexOf('`'))}<{argumentList}>";
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
            return parameterInfo.Name!;
        }

        return (modelNameProvider.Name ?? parameterInfo.Name)!;
    }

    private static string GetRootPath(
        [NotNull] Type controllerType,
        [NotNull] ActionDescriptor actionDescriptor,
        ConventionalControllerSetting? setting)
    {
        if (setting != null)
        {
            return setting.RootPath;
        }

        var areaAttr = controllerType.GetCustomAttributes().OfType<AreaAttribute>().FirstOrDefault() ?? actionDescriptor.EndpointMetadata.OfType<AreaAttribute>().FirstOrDefault();
        if (areaAttr != null)
        {
            return areaAttr.RouteValue;
        }

        return ModuleApiDescriptionModel.DefaultRootPath;
    }

    private string GetRemoteServiceName(Type controllerType, ConventionalControllerSetting? setting)
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

    private string? FindGroupName(Type controllerType)
    {
        var controllerNameAttribute =
            controllerType.GetCustomAttributes().OfType<ControllerNameAttribute>().FirstOrDefault();

        if (controllerNameAttribute?.Name != null)
        {
            return controllerNameAttribute.Name;
        }

        return null;
    }

    private ConventionalControllerSetting? FindSetting(Type controllerType)
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

    protected virtual async Task PopulateControllerDescriptionsAsync(ControllerApiDescriptionModel controllerModel, Type controllerType)
    {
        controllerModel.Summary = await _xmlDocProvider.GetSummaryAsync(controllerType);
        controllerModel.Remarks = await _xmlDocProvider.GetRemarksAsync(controllerType);

        if (controllerModel.Summary == null && controllerModel.Remarks == null)
        {
            foreach (var interfaceType in GetDirectInterfaces(controllerType).Where(i => !_modelOptions.IgnoredInterfaces.Contains(i)))
            {
                controllerModel.Summary = await _xmlDocProvider.GetSummaryAsync(interfaceType);
                controllerModel.Remarks = await _xmlDocProvider.GetRemarksAsync(interfaceType);
                if (controllerModel.Summary != null || controllerModel.Remarks != null)
                {
                    break;
                }
            }
        }

        controllerModel.Description = controllerType.GetCustomAttribute<DescriptionAttribute>()?.Description;
        controllerModel.DisplayName = controllerType.GetCustomAttribute<DisplayAttribute>()?.Name;
    }

    protected virtual async Task PopulateActionDescriptionsAsync(ActionApiDescriptionModel actionModel, MethodInfo method, MethodInfo? interfaceMethod)
    {
        actionModel.Summary = await _xmlDocProvider.GetSummaryAsync(method);
        actionModel.Remarks = await _xmlDocProvider.GetRemarksAsync(method);

        if (actionModel.Summary == null && actionModel.Remarks == null && interfaceMethod != null)
        {
            actionModel.Summary = await _xmlDocProvider.GetSummaryAsync(interfaceMethod);
            actionModel.Remarks = await _xmlDocProvider.GetRemarksAsync(interfaceMethod);
        }

        actionModel.Description = method.GetCustomAttribute<DescriptionAttribute>()?.Description;
        actionModel.DisplayName = method.GetCustomAttribute<DisplayAttribute>()?.Name;

        actionModel.ReturnValue.Summary = await _xmlDocProvider.GetReturnsAsync(method);
        if (actionModel.ReturnValue.Summary == null && interfaceMethod != null)
        {
            actionModel.ReturnValue.Summary = await _xmlDocProvider.GetReturnsAsync(interfaceMethod);
        }
    }

    protected virtual async Task PopulateParameterDescriptionsAsync(ActionApiDescriptionModel actionModel, MethodInfo method, MethodInfo? interfaceMethod)
    {
        var methodParameters = method.GetParameters();

        foreach (var param in actionModel.ParametersOnMethod)
        {
            var paramInfo = methodParameters.FirstOrDefault(p => p.Name == param.Name);
            if (paramInfo == null)
            {
                continue;
            }

            param.Summary = await _xmlDocProvider.GetParameterSummaryAsync(method, param.Name);
            if (param.Summary == null && interfaceMethod != null)
            {
                param.Summary = await _xmlDocProvider.GetParameterSummaryAsync(interfaceMethod, param.Name);
            }

            param.Description = paramInfo.GetCustomAttribute<DescriptionAttribute>()?.Description;
            param.DisplayName = paramInfo.GetCustomAttribute<DisplayAttribute>()?.Name;
        }

        foreach (var param in actionModel.Parameters)
        {
            // Skip expanded properties from complex types - their descriptions
            // should come from type-level documentation (PopulateTypeDescriptionsAsync)
            if (!string.IsNullOrEmpty(param.DescriptorName) && param.Name != param.NameOnMethod)
            {
                continue;
            }

            param.Summary = await _xmlDocProvider.GetParameterSummaryAsync(method, param.NameOnMethod);
            if (param.Summary == null && interfaceMethod != null)
            {
                param.Summary = await _xmlDocProvider.GetParameterSummaryAsync(interfaceMethod, param.NameOnMethod);
            }

            var paramInfo = methodParameters.FirstOrDefault(p => p.Name == param.NameOnMethod);
            if (paramInfo != null)
            {
                param.Description = paramInfo.GetCustomAttribute<DescriptionAttribute>()?.Description;
                param.DisplayName = paramInfo.GetCustomAttribute<DisplayAttribute>()?.Name;
            }
        }
    }

    private MethodInfo? GetInterfaceMethod(MethodInfo method)
    {
        var declaringType = method.DeclaringType;
        if (declaringType == null || declaringType.IsInterface)
        {
            return null;
        }

        foreach (var interfaceType in GetDirectInterfaces(declaringType).Where(i => !_modelOptions.IgnoredInterfaces.Contains(i)))
        {
            var map = declaringType.GetInterfaceMap(interfaceType);
            for (var i = 0; i < map.TargetMethods.Length; i++)
            {
                if (map.TargetMethods[i] == method)
                {
                    return map.InterfaceMethods[i];
                }
            }
        }

        return null;
    }

    private static IEnumerable<Type> GetDirectInterfaces(Type type)
    {
        var allInterfaces = type.GetInterfaces();
        var baseInterfaces = type.BaseType?.GetInterfaces() ?? Type.EmptyTypes;
        return allInterfaces.Except(baseInterfaces);
    }

    protected virtual async Task PopulateTypeDescriptionsAsync(TypeApiDescriptionModel typeModel, Type type)
    {
        typeModel.Summary = await _xmlDocProvider.GetSummaryAsync(type);
        typeModel.Remarks = await _xmlDocProvider.GetRemarksAsync(type);
        typeModel.Description = type.GetCustomAttribute<DescriptionAttribute>()?.Description;
        typeModel.DisplayName = type.GetCustomAttribute<DisplayAttribute>()?.Name;

        if (typeModel.Properties == null)
        {
            return;
        }

        foreach (var propModel in typeModel.Properties)
        {
            var propInfo = type.GetProperty(propModel.Name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
            if (propInfo == null)
            {
                continue;
            }

            propModel.Summary = await _xmlDocProvider.GetSummaryAsync(propInfo);
            propModel.Description = propInfo.GetCustomAttribute<DescriptionAttribute>()?.Description;
            propModel.DisplayName = propInfo.GetCustomAttribute<DisplayAttribute>()?.Name;
        }
    }
}
