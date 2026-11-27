using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Authorization;
using Volo.Abp.DynamicProxy;
using Volo.Abp.Features;

namespace Volo.Abp.AgentTools;

/// <summary>Checks the invoker and the provider share, beyond what ABP's interceptors cover.</summary>
internal static class AgentToolAuthorizationChecks
{
    /// <summary>
    /// A hand-written controller is not on the in-process path, so its [Authorize] / [RequiresFeature] would be bypassed.
    /// Checks them with ABP's services: the action method (and its declaring class), plus the class-level attributes of the
    /// actual controller type when the action is declared on a base controller.
    /// Not applied in-process: global MVC authorization (AuthorizeFilter, the fallback policy, endpoint RequireAuthorization).
    /// Throws AbpAuthorizationException when refused.
    /// </summary>
    public static async Task CheckControllerAsync(IServiceProvider services, AgentToolDefinition definition)
    {
        var action = definition.ControllerActionMethod;
        if (action == null)
        {
            return;
        }

        await services.GetRequiredService<IMethodInvocationAuthorizationService>()
            .CheckAsync(new MethodInvocationAuthorizationContext(action));
        await services.GetRequiredService<IMethodInvocationFeatureCheckerService>()
            .CheckAsync(new MethodInvocationFeatureCheckerContext(action));

        var controllerType = definition.ControllerType;
        if (controllerType == null || controllerType == action.DeclaringType)
        {
            return;
        }

        var authorizeData = controllerType.GetCustomAttributes(true).OfType<IAuthorizeData>().ToList();
        if (authorizeData.Count > 0)
        {
            var policy = await AuthorizationPolicy.CombineAsync(services.GetRequiredService<IAbpAuthorizationPolicyProvider>(), authorizeData);
            if (policy != null)
            {
                await services.GetRequiredService<IAuthorizationService>().CheckAsync(policy);
            }
        }

        foreach (var requiresFeature in controllerType.GetCustomAttributes<RequiresFeatureAttribute>(true))
        {
            await services.GetRequiredService<IFeatureChecker>().CheckEnabledAsync(requiresFeature.RequiresAll, requiresFeature.Features);
        }
    }

    /// <summary>
    /// Narrow adapter for ABP's CRUD application services (AbstractKeyReadOnlyAppService / AbstractKeyCrudAppService and
    /// everything derived, e.g. CrudAppService): their standard Get, GetList, Create, Update and Delete methods check
    /// GetPolicyName, GetListPolicyName, CreatePolicyName, UpdatePolicyName and DeletePolicyName inside the method body,
    /// which no attribute shows. Reads that property from an instance of the service and checks it, for the tool list only;
    /// a call is checked by the service itself. Returns the policy name, or null when the adapter does not apply / none is set.
    /// Other checks inside method bodies stay invisible to the tool list.
    /// </summary>
    public static string? FindCrudPolicyName(AgentToolDefinition definition, Func<object> resolveService)
    {
        var declaring = definition.ImplementationMethod.GetBaseDefinition().DeclaringType;
        if (declaring is not { IsGenericType: true }
            || declaring.Namespace != "Volo.Abp.Application.Services"
            || !(declaring.Name.StartsWith("AbstractKeyReadOnlyAppService`") || declaring.Name.StartsWith("AbstractKeyCrudAppService`")))
        {
            return null;
        }

        var name = definition.ImplementationMethod.Name.RemovePostFix("Async");
        if (name is not ("Get" or "GetList" or "Create" or "Update" or "Delete"))
        {
            return null;
        }

        var instance = ProxyHelper.UnProxy(resolveService());
        return instance.GetType()
            .GetProperty(name + "PolicyName", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)?
            .GetValue(instance) as string;
    }
}
