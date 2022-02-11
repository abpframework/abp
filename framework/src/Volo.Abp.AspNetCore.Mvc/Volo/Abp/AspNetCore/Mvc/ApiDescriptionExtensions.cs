using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Volo.Abp.Reflection;

namespace Volo.Abp.AspNetCore.Mvc;

public static class ApiDescriptionExtensions
{
    public static bool IsRemoteService(this ApiDescription actionDescriptor)
    {
        if (actionDescriptor.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
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
}
