using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Volo.Abp.AspNetCore.Mvc
{
    public static class ActionResultHelper
    {
        public static bool IsObjectResult(Type returnType)
        {
            //Get the actual return type (unwrap Task)
            if (returnType == typeof(Task))
            {
                returnType = typeof(void);
            }
            else if (returnType.GetTypeInfo().IsGenericType && returnType.GetGenericTypeDefinition() == typeof(Task<>))
            {
                returnType = returnType.GenericTypeArguments[0];
            }

            if (typeof(IActionResult).GetTypeInfo().IsAssignableFrom(returnType))
            {
                if (typeof(JsonResult).GetTypeInfo().IsAssignableFrom(returnType) || typeof(ObjectResult).GetTypeInfo().IsAssignableFrom(returnType))
                {
                    return true;
                }

                return false;
            }

            return true;
        }
    }
}