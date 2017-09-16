using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using Volo.Abp.Application.Services;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http;
using Volo.Abp.Http.Modeling;
using Volo.Abp.Reflection;

namespace Volo.Abp.AspNetCore.Mvc
{
    public class AbpAppServiceConvention : IAbpAppServiceConvention, ITransientDependency
    {
        private readonly AbpAspNetCoreMvcOptions _options;

        public AbpAppServiceConvention(IOptions<AbpAspNetCoreMvcOptions> options)
        {
            _options = options.Value;
        }

        public void Apply(ApplicationModel application)
        {
            ApplyForControllers(application);
        }

        protected virtual void ApplyForControllers(ApplicationModel application)
        {
            foreach (var controller in application.Controllers)
            {
                var controllerType = controller.ControllerType.AsType();
                var configuration = GetControllerSettingOrNull(controllerType);

                if (IsApplicationService(controllerType))
                {
                    controller.ControllerName = controller.ControllerName.RemovePostFix(ApplicationService.CommonPostfixes);
                    configuration?.ControllerModelConfigurer(controller);
                    ConfigureArea(controller, configuration);
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

        protected virtual void ConfigureArea(ControllerModel controller, [CanBeNull] AbpControllerAssemblySetting configuration)
        {
            if (configuration == null)
            {
                return;
            }

            if (controller.RouteValues.ContainsKey("area"))
            {
                return;
            }

            controller.RouteValues["area"] = configuration.ModuleName;
        }

        protected virtual void ConfigureRemoteService(ControllerModel controller, [CanBeNull] AbpControllerAssemblySetting configuration)
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

                    if (!TypeHelper.IsPrimitiveExtendedIncludingNullable(prm.ParameterInfo.ParameterType))
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
            if (_options.AppServiceControllers.FormBodyBindingIgnoredTypes.Any(t => t.IsAssignableFrom(parameter.ParameterInfo.ParameterType)))
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
                var controllerType = controller.ControllerType.AsType();
                var remoteServiceAtt = ReflectionHelper.GetSingleAttributeOrDefault<RemoteServiceAttribute>(controllerType.GetTypeInfo());
                if (remoteServiceAtt != null)
                {
                    controller.ApiExplorer.IsVisible =
                        remoteServiceAtt.IsEnabledFor(controllerType) &&
                        remoteServiceAtt.IsMetadataEnabledFor(controllerType);
                }
                else
                {
                    controller.ApiExplorer.IsVisible = true;
                }
            }

            foreach (var action in controller.Actions)
            {
                ConfigureApiExplorer(action);
            }
        }

        protected virtual void ConfigureApiExplorer(ActionModel action)
        {
            if (action.ApiExplorer.IsVisible == null)
            {
                var remoteServiceAtt = ReflectionHelper.GetSingleAttributeOrDefault<RemoteServiceAttribute>(action.ActionMethod);
                if (remoteServiceAtt != null)
                {
                    action.ApiExplorer.IsVisible =
                        remoteServiceAtt.IsEnabledFor(action.ActionMethod) &&
                        remoteServiceAtt.IsMetadataEnabledFor(action.ActionMethod);
                }
            }
        }

        protected virtual void ConfigureSelector(ControllerModel controller, [CanBeNull] AbpControllerAssemblySetting configuration)
        {
            RemoveEmptySelectors(controller.Selectors);

            if (controller.Selectors.Any(selector => selector.AttributeRouteModel != null))
            {
                return;
            }

            var moduleName = GetModuleNameOrDefault(controller.ControllerType.AsType());

            foreach (var action in controller.Actions)
            {
                ConfigureSelector(moduleName, controller.ControllerName, action, configuration);
            }
        }

        protected virtual void ConfigureSelector(string moduleName, string controllerName, ActionModel action, [CanBeNull] AbpControllerAssemblySetting configuration)
        {
            RemoveEmptySelectors(action.Selectors);

            if (!action.Selectors.Any())
            {
                AddAbpServiceSelector(moduleName, controllerName, action, configuration);
            }
            else
            {
                NormalizeSelectorRoutes(moduleName, controllerName, action);
            }
        }

        protected virtual void AddAbpServiceSelector(string moduleName, string controllerName, ActionModel action, [CanBeNull] AbpControllerAssemblySetting configuration)
        {
            var httpMethod = SelectHttpMethod(action, configuration);

            var abpServiceSelectorModel = new SelectorModel
            {
                AttributeRouteModel = CreateAbpServiceAttributeRouteModel(moduleName, controllerName, action, httpMethod),
                ActionConstraints = { new HttpMethodActionConstraint(new[] { httpMethod }) }
            };

            action.Selectors.Add(abpServiceSelectorModel);
        }

        protected virtual string SelectHttpMethod(ActionModel action, AbpControllerAssemblySetting configuration)
        {
            return configuration?.UseConventionalHttpVerbs == true
                ? HttpMethodHelper.GetConventionalVerbForMethodName(action.ActionName)
                : HttpMethodHelper.DefaultHttpVerb;
        }

        protected virtual void NormalizeSelectorRoutes(string moduleName, string controllerName, ActionModel action)
        {
            foreach (var selector in action.Selectors)
            {
                var httpMethod = selector.ActionConstraints.OfType<HttpMethodActionConstraint>().FirstOrDefault()?.HttpMethods?.FirstOrDefault();
                if (selector.AttributeRouteModel == null)
                {
                    selector.AttributeRouteModel = CreateAbpServiceAttributeRouteModel(moduleName, controllerName, action, httpMethod);
                }
            }
        }

        protected virtual string GetModuleNameOrDefault(Type controllerType)
        {
            return GetControllerSettingOrNull(controllerType)?.ModuleName ??
                   ModuleApiDescriptionModel.DefaultServiceModuleName;
        }

        [CanBeNull]
        protected virtual AbpControllerAssemblySetting GetControllerSettingOrNull(Type controllerType)
        {
            return _options.AppServiceControllers.ControllerAssemblySettings.GetSettingOrNull(controllerType);
        }

        protected virtual AttributeRouteModel CreateAbpServiceAttributeRouteModel(string moduleName, string controllerName, ActionModel action, string httpMethod)
        {
            var url = CalculateUrl(moduleName, controllerName, action, httpMethod);

            return new AttributeRouteModel(new RouteAttribute(url));
        }

        protected virtual string CalculateUrl(string moduleName, string controllerName, ActionModel action, string httpMethod)
        {
            var url = $"api/{moduleName}/{controllerName}";

            //Add {id} path if needed
            if (action.Parameters.Any(p => p.ParameterName == "id"))
            {
                url += "/{id}";
            }
            
            //Add action name if needed
            var actionNameInUrl = NormalizeUrlActionName(moduleName, controllerName, action, httpMethod);
            if (!actionNameInUrl.IsNullOrEmpty())
            {
                url += $"/{actionNameInUrl}";

                //Add secondary Id
                var secondaryIds = action.Parameters.Where(p => p.ParameterName.EndsWith("Id", StringComparison.Ordinal)).ToList();
                if (secondaryIds.Count == 1)
                {
                    url += $"/{{{secondaryIds[0].ParameterName}}}";
                }
            }

            return url;
        }

        protected virtual string NormalizeUrlActionName(string moduleName, string controllerName, ActionModel action, string httpMethod)
        {
            var context = new UrlActionNameNormalizerContext(moduleName, controllerName, action, httpMethod, action.ActionName);

            foreach (var normalizer in _options.AppServiceControllers.UrlActionNameNormalizers)
            {
                normalizer.Normalize(context);
            }

            return context.ActionNameInUrl;
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
            return selector.AttributeRouteModel == null && selector.ActionConstraints.IsNullOrEmpty();
        }

        protected virtual bool IsApplicationService(Type controllerType)
        {
            return typeof(IApplicationService).GetTypeInfo().IsAssignableFrom(controllerType);
        }
    }
}