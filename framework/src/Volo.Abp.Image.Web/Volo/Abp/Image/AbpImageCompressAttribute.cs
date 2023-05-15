using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Volo.Abp.Content;

namespace Volo.Abp.Image;

public class AbpImageCompressActionFilterAttribute : ActionFilterAttribute
{
    public string[] Parameters { get; }

    public AbpImageCompressActionFilterAttribute(params string[] parameters)
    {
        Parameters = parameters;
    }

    public async override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var parameters = Parameters.Any()
            ? context.ActionArguments.Where(x => Parameters.Contains(x.Key)).ToArray()
            : context.ActionArguments.ToArray();

        var parameterDescriptors = context.ActionDescriptor.Parameters.OfType<ControllerParameterDescriptor>();
        var parameterMap = new Dictionary<string, ParameterInfo>();
        foreach (var parameterDescriptor in parameterDescriptors)
        {
            var parameter = parameterDescriptor.ParameterInfo;
            var attribute = parameter.GetCustomAttribute<AbpOriginalImageAttribute>();
            if (attribute == null)
            {
                continue;
            }

            parameterMap.Add(attribute.Parameter, parameter);
        }

        foreach (var (key, value) in parameters)
        {
            object compressedValue = value switch {
                IFormFile file => await file.CompressImageAsync(context.HttpContext.RequestServices),
                IRemoteStreamContent remoteStreamContent => await remoteStreamContent.CompressImageAsync(context.HttpContext.RequestServices),
                Stream stream => await stream.CompressImageAsync(context.HttpContext.RequestServices),
                _ => null
            };

            if (parameterMap.TryGetValue(key, out var parameterInfo))
            {
                if (parameterInfo.Name != null)
                {
                    context.ActionArguments.Add(parameterInfo.Name, value);
                }
                else if (value is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }
            else if (value is IDisposable disposable)
            {
                disposable.Dispose();
            }

            if (compressedValue != null)
            {
                context.ActionArguments[key] = compressedValue;
            }
        }

        await next();
    }
}