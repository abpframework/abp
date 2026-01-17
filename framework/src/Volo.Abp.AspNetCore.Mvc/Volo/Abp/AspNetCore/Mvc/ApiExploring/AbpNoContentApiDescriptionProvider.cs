using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Reflection;

namespace Volo.Abp.AspNetCore.Mvc.ApiExploring;

public class AbpNoContentApiDescriptionProvider : IApiDescriptionProvider, ITransientDependency
{
    public virtual void OnProvidersExecuted(ApiDescriptionProviderContext context)
    {
    }

    /// <summary>
    /// The order -999 ensures that this provider is executed right after the
    /// Microsoft.AspNetCore.Mvc.ApiExplorer.DefaultApiDescriptionProvider.
    /// </summary>
    public int Order => -999;

    public virtual void OnProvidersExecuting(ApiDescriptionProviderContext context)
    {
        foreach (var result in context.Results.Where(x => x.IsRemoteService()))
        {
            var actionProducesResponseTypeAttributes =
                ReflectionHelper.GetAttributesOfMemberOrDeclaringType<ProducesResponseTypeAttribute>(
                    result.ActionDescriptor.GetMethodInfo());
            if (actionProducesResponseTypeAttributes.Any(x => x.StatusCode == (int) HttpStatusCode.NoContent))
            {
                continue;
            }

            var returnType = result.ActionDescriptor.GetReturnType();
            if (returnType == typeof(Task) || returnType == typeof(void))
            {
                result.SupportedResponseTypes.Add(new ApiResponseType
                {
                    // If the return type is Task, then we should treat it as a void return type since we can't infer anything without additional metadata or requiring unreferenced code.
                    Type = typeof(void),
                    StatusCode = (int) HttpStatusCode.NoContent
                });
            }
        }
    }
}
