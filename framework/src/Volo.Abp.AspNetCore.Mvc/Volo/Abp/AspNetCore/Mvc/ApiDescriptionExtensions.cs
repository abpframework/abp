using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Volo.Abp.Reflection;

namespace Volo.Abp.AspNetCore.Mvc;

public static class ApiDescriptionExtensions
{
    public static bool IsRemoteService(this ApiDescription apiDescriptor)
    {
        if (apiDescriptor.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
        {
            var remoteServiceAttr = ReflectionHelper.GetSingleAttributeOfMemberOrDeclaringTypeOrDefault<RemoteServiceAttribute>(controllerActionDescriptor.MethodInfo);
            if (remoteServiceAttr != null && remoteServiceAttr.IsEnabled)
            {
                return true;
            }

            remoteServiceAttr = ReflectionHelper.GetSingleAttributeOfMemberOrDeclaringTypeOrDefault<RemoteServiceAttribute>(controllerActionDescriptor.ControllerTypeInfo);
            if (remoteServiceAttr != null && remoteServiceAttr.IsEnabled)
            {
                return true;
            }

            if (typeof(IRemoteService).IsAssignableFrom(controllerActionDescriptor.ControllerTypeInfo))
            {
                return true;
            }
        }

        return false;
    }

    public static bool IsIntegrationService(this ApiDescription apiDescriptor)
    {
        if (apiDescriptor.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
        {
            return  IntegrationServiceAttribute.IsDefinedOrInherited(controllerActionDescriptor.ControllerTypeInfo);
        }

        return false;
    }
}
