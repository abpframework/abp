using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;

namespace Volo.Abp.AspNetCore.Mvc.DependencyInjection
{
    /// <summary>
    /// Creates controllers using the dependency injection system.
    /// Fallbacks to the default controller activator if given controller type
    /// was not registered to DI.
    /// </summary>
    public class AbpServiceBasedControllerActivator : IControllerActivator
    {
        private readonly Type _innerActivatorType;

        public AbpServiceBasedControllerActivator(Type innerActivatorType)
        {
            _innerActivatorType = innerActivatorType;
        }
        
        public object Create(ControllerContext context)
        {
            Check.NotNull(context, nameof(context));

            var controllerType = context.ActionDescriptor.ControllerTypeInfo;
            var controllerInstance = context.HttpContext.RequestServices.GetService(controllerType);
            if (controllerInstance != null)
            {
                return controllerInstance;
            }

            var innerActivator = (IControllerActivator)context.HttpContext.RequestServices.GetRequiredService(_innerActivatorType);
            controllerInstance = innerActivator.Create(context);

            var controllerInstances = (HashSet<object>) context.HttpContext.Items.GetOrAdd(
                "_AbpNonRegisteredControllerInstances", 
                () => new HashSet<object>());
            
            Debug.Assert(controllerInstances != null, "controllerInstances != null");
            controllerInstances.Add(controllerInstance);
            
            return controllerInstance;
        }

        public void Release(ControllerContext context, object controller)
        {
            var controllerInstances = context.HttpContext.Items.GetOrDefault(
                "_AbpNonRegisteredControllerInstances"
                ) as HashSet<object>;

            if (controllerInstances != null && controllerInstances.Contains(controller))
            {
                var innerActivator = (IControllerActivator)context.HttpContext.RequestServices.GetRequiredService(_innerActivatorType);
                innerActivator.Release(context, controller);
                controllerInstances.Remove(controller);
            }
        }
    }
}