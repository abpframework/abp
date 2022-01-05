using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Volo.Abp.Application.Services;
using Volo.Abp.DependencyInjection;
using Volo.Abp.GlobalFeatures;
using Volo.Abp.Http;
using Volo.Abp.Http.Modeling;
using Volo.Abp.Reflection;

namespace Volo.Abp.AspNetCore.Mvc.Conventions;

public class AbpServiceConvention : IAbpServiceConvention, ITransientDependency
{
    public ILogger<AbpServiceConvention> Logger { get; set; }

    protected AbpAspNetCoreMvcOptions Options { get; }
    protected IConventionalRouteBuilder ConventionalRouteBuilder { get; }

    public AbpServiceConvention(
        IOptions<AbpAspNetCoreMvcOptions> options,
        IConventionalRouteBuilder conventionalRouteBuilder)
    {
        ConventionalRouteBuilder = conventionalRouteBuilder;
        Options = options.Value;

        Logger = NullLogger<AbpServiceConvention>.Instance;
    }

    public void Apply(ApplicationModel application)
    {
        ApplyForControllers(application);
    }

    protected virtual void ApplyForControllers(ApplicationModel application)
    {
        RemoveDuplicateControllers(application);

        foreach (var controller in GetControllers(application))
        {
            var controllerType = controller.ControllerType.AsType();

            var configuration = GetControllerSettingOrNull(controllerType);

            //TODO: We can remove different behaviour for ImplementsRemoteServiceInterface. If there is a configuration, then it should be applied!
            //TODO: But also consider ConventionalControllerSetting.IsRemoteService method too..!

            if (ImplementsRemoteServiceInterface(controllerType))
            {
                controller.ControllerName = controller.ControllerName.RemovePostFix(ApplicationService.CommonPostfixes);
                configuration?.ControllerModelConfigurer?.Invoke(controller);
                ConfigureRemoteService(controller, configuration);
            }
            else
            {
                var remoteServiceAttr = ReflectionHelper.GetSingleAttributeOrDefault<RemoteServiceAttribute>(controllerType.GetTypeInfo());
                if (remoteServiceAttr != null && remoteServiceAttr.IsEnabledFor(controllerType))
                {
                    ConfigureRemoteService(controller, configuration);
                }
            }
        }
    }

    protected virtual IList<ControllerModel> GetControllers(ApplicationModel application)
    {
        return application.Controllers;
    }

    protected virtual void RemoveDuplicateControllers(ApplicationModel application)
    {
        var controllerModelsToRemove = new List<ControllerModel>();

        foreach (var controllerModel in GetControllers(application))
        {
            if (!controllerModel.ControllerType.IsDefined(typeof(ExposeServicesAttribute), false))
            {
                continue;
            }

            if (Options.IgnoredControllersOnModelExclusion.Contains(controllerModel.ControllerType))
            {
                continue;
            }

            var exposeServicesAttr = ReflectionHelper.GetSingleAttributeOrDefault<ExposeServicesAttribute>(controllerModel.ControllerType);
            if (exposeServicesAttr.IncludeSelf)
            {
                var exposedControllerModels = GetControllers(application)
                    .Where(cm => exposeServicesAttr.ServiceTypes.Contains(cm.ControllerType))
                    .ToArray();

                controllerModelsToRemove.AddRange(exposedControllerModels);
                Logger.LogInformation($"Removing the controller{(exposedControllerModels.Length > 1 ? "s" : "")} {exposeServicesAttr.ServiceTypes.Select(c => c.AssemblyQualifiedName).JoinAsString(", ")} from the application model since {(exposedControllerModels.Length > 1 ? "they are" : "it is")} replaced by the controller: {controllerModel.ControllerType.AssemblyQualifiedName}");
                continue;
            }

            var baseControllerTypes = controllerModel.ControllerType
                .GetBaseClasses(typeof(Controller), includeObject: false)
                .Where(t => !t.IsAbstract)
                .ToArray();

            if (baseControllerTypes.Length == 0)
            {
                continue;
            }

            var baseControllerModels = GetControllers(application)
                .Where(cm => baseControllerTypes.Contains(cm.ControllerType))
                .ToArray();

            if (baseControllerModels.Length == 0)
            {
                continue;
            }

            controllerModelsToRemove.Add(controllerModel);
            Logger.LogInformation($"Removing the controller {controllerModel.ControllerType.AssemblyQualifiedName} from the application model since it replaces the controller(s): {baseControllerTypes.Select(c => c.AssemblyQualifiedName).JoinAsString(", ")}");
        }

        application.Controllers.RemoveAll(controllerModelsToRemove);
    }

    protected virtual void ConfigureRemoteService(ControllerModel controller, [CanBeNull] ConventionalControllerSetting configuration)
    {
        ConfigureApiExplorer(controller);
        ConfigureSelector(controller, configuration);
        ConfigureParameters(controller);
    }

    protected virtual void ConfigureParameters(ControllerModel controller)
    {
        /* Default binding system of Asp.Net Core for a parameter
         * 1. Form values
         * 2. Route values.
         * 3. Query string.
         */

        foreach (var action in controller.Actions)
        {
            foreach (var prm in action.Parameters)
            {
                if (prm.BindingInfo != null)
                {
                    continue;
                }

                if (!TypeHelper.IsPrimitiveExtended(prm.ParameterInfo.ParameterType, includeEnums: true))
                {
                    if (CanUseFormBodyBinding(action, prm))
                    {
                        prm.BindingInfo = BindingInfo.GetBindingInfo(new[] { new FromBodyAttribute() });
                    }
                }
            }
        }
    }

    protected virtual bool CanUseFormBodyBinding(ActionModel action, ParameterModel parameter)
    {
        //We want to use "id" as path parameter, not body!
        if (parameter.ParameterName == "id")
        {
            return false;
        }

        if (Options.ConventionalControllers
            .FormBodyBindingIgnoredTypes
            .Any(t => t.IsAssignableFrom(parameter.ParameterInfo.ParameterType)))
        {
            return false;
        }

        foreach (var selector in action.Selectors)
        {
            if (selector.ActionConstraints == null)
            {
                continue;
            }

            foreach (var actionConstraint in selector.ActionConstraints)
            {
                var httpMethodActionConstraint = actionConstraint as HttpMethodActionConstraint;
                if (httpMethodActionConstraint == null)
                {
                    continue;
                }

                if (httpMethodActionConstraint.HttpMethods.All(hm => hm.IsIn("GET", "DELETE", "TRACE", "HEAD")))
                {
                    return false;
                }
            }
        }

        return true;
    }

    protected virtual void ConfigureApiExplorer(ControllerModel controller)
    {
        if (controller.ApiExplorer.GroupName.IsNullOrEmpty())
        {
            controller.ApiExplorer.GroupName = controller.ControllerName;
        }

        if (controller.ApiExplorer.IsVisible == null)
        {
            controller.ApiExplorer.IsVisible = IsVisibleRemoteService(controller.ControllerType);
        }

        foreach (var action in controller.Actions)
        {
            ConfigureApiExplorer(action);
        }
    }

    protected virtual void ConfigureApiExplorer(ActionModel action)
    {
        if (action.ApiExplorer.IsVisible != null)
        {
            return;
        }

        var visible = IsVisibleRemoteServiceMethod(action.ActionMethod);
        if (visible == null)
        {
            return;
        }

        action.ApiExplorer.IsVisible = visible;
    }

    protected virtual void ConfigureSelector(ControllerModel controller, [CanBeNull] ConventionalControllerSetting configuration)
    {
        RemoveEmptySelectors(controller.Selectors);

        var controllerType = controller.ControllerType.AsType();
        var remoteServiceAtt = ReflectionHelper.GetSingleAttributeOrDefault<RemoteServiceAttribute>(controllerType.GetTypeInfo());
        if (remoteServiceAtt != null && !remoteServiceAtt.IsEnabledFor(controllerType))
        {
            return;
        }

        if (controller.Selectors.Any(selector => selector.AttributeRouteModel != null))
        {
            return;
        }

        var rootPath = GetRootPathOrDefault(controller.ControllerType.AsType());

        foreach (var action in controller.Actions)
        {
            ConfigureSelector(rootPath, controller.ControllerName, action, configuration);
        }
    }

    protected virtual void ConfigureSelector(string rootPath, string controllerName, ActionModel action, [CanBeNull] ConventionalControllerSetting configuration)
    {
        RemoveEmptySelectors(action.Selectors);

        var remoteServiceAtt = ReflectionHelper.GetSingleAttributeOrDefault<RemoteServiceAttribute>(action.ActionMethod);
        if (remoteServiceAtt != null && !remoteServiceAtt.IsEnabledFor(action.ActionMethod))
        {
            return;
        }

        if (!action.Selectors.Any())
        {
            AddAbpServiceSelector(rootPath, controllerName, action, configuration);
        }
        else
        {
            NormalizeSelectorRoutes(rootPath, controllerName, action, configuration);
        }
    }

    protected virtual void AddAbpServiceSelector(string rootPath, string controllerName, ActionModel action, [CanBeNull] ConventionalControllerSetting configuration)
    {
        var httpMethod = SelectHttpMethod(action, configuration);

        var abpServiceSelectorModel = new SelectorModel
        {
            AttributeRouteModel = CreateAbpServiceAttributeRouteModel(rootPath, controllerName, action, httpMethod, configuration),
            ActionConstraints = { new HttpMethodActionConstraint(new[] { httpMethod }) }
        };

        action.Selectors.Add(abpServiceSelectorModel);
    }

    protected virtual string SelectHttpMethod(ActionModel action, ConventionalControllerSetting configuration)
    {
        return HttpMethodHelper.GetConventionalVerbForMethodName(action.ActionName);
    }

    protected virtual void NormalizeSelectorRoutes(string rootPath, string controllerName, ActionModel action, [CanBeNull] ConventionalControllerSetting configuration)
    {
        foreach (var selector in action.Selectors)
        {
            var httpMethod = selector.ActionConstraints
                .OfType<HttpMethodActionConstraint>()
                .FirstOrDefault()?
                .HttpMethods?
                .FirstOrDefault();

            if (httpMethod == null)
            {
                httpMethod = SelectHttpMethod(action, configuration);
            }

            if (selector.AttributeRouteModel == null)
            {
                selector.AttributeRouteModel = CreateAbpServiceAttributeRouteModel(rootPath, controllerName, action, httpMethod, configuration);
            }

            if (!selector.ActionConstraints.OfType<HttpMethodActionConstraint>().Any())
            {
                selector.ActionConstraints.Add(new HttpMethodActionConstraint(new[] { httpMethod }));
            }
        }
    }

    protected virtual string GetRootPathOrDefault(Type controllerType)
    {
        var controllerSetting = GetControllerSettingOrNull(controllerType);
        if (controllerSetting?.RootPath != null)
        {
            return controllerSetting.RootPath;
        }

        var areaAttribute = controllerType.GetCustomAttributes().OfType<AreaAttribute>().FirstOrDefault();
        if (areaAttribute?.RouteValue != null)
        {
            return areaAttribute.RouteValue;
        }

        return ModuleApiDescriptionModel.DefaultRootPath;
    }

    [CanBeNull]
    protected virtual ConventionalControllerSetting GetControllerSettingOrNull(Type controllerType)
    {
        return Options.ConventionalControllers.ConventionalControllerSettings.GetSettingOrNull(controllerType);
    }

    protected virtual AttributeRouteModel CreateAbpServiceAttributeRouteModel(string rootPath, string controllerName, ActionModel action, string httpMethod, [CanBeNull] ConventionalControllerSetting configuration)
    {
        return new AttributeRouteModel(
            new RouteAttribute(
                ConventionalRouteBuilder.Build(rootPath, controllerName, action, httpMethod, configuration)
            )
        );
    }

    protected virtual void RemoveEmptySelectors(IList<SelectorModel> selectors)
    {
        selectors
            .Where(IsEmptySelector)
            .ToList()
            .ForEach(s => selectors.Remove(s));
    }

    protected virtual bool IsEmptySelector(SelectorModel selector)
    {
        return selector.AttributeRouteModel == null
               && selector.ActionConstraints.IsNullOrEmpty()
               && selector.EndpointMetadata.IsNullOrEmpty();
    }

    protected virtual bool ImplementsRemoteServiceInterface(Type controllerType)
    {
        return typeof(IRemoteService).GetTypeInfo().IsAssignableFrom(controllerType);
    }

    protected virtual bool IsVisibleRemoteService(Type controllerType)
    {
        if (!IsGlobalFeatureEnabled(controllerType))
        {
            return false;
        }

        var attribute = ReflectionHelper.GetSingleAttributeOrDefault<RemoteServiceAttribute>(controllerType);
        if (attribute == null)
        {
            return true;
        }

        return attribute.IsEnabledFor(controllerType) &&
               attribute.IsMetadataEnabledFor(controllerType);
    }

    protected virtual bool? IsVisibleRemoteServiceMethod(MethodInfo method)
    {
        var attribute = ReflectionHelper.GetSingleAttributeOrDefault<RemoteServiceAttribute>(method);
        if (attribute == null)
        {
            return null;
        }

        return attribute.IsEnabledFor(method) &&
               attribute.IsMetadataEnabledFor(method);
    }

    protected virtual bool IsGlobalFeatureEnabled(Type controllerType)
    {
        var attribute = ReflectionHelper.GetSingleAttributeOrDefault<RequiresGlobalFeatureAttribute>(controllerType);
        if (attribute == null)
        {
            return true;
        }

        return GlobalFeatureManager.Instance.IsEnabled(attribute.GetFeatureName());
    }
}
