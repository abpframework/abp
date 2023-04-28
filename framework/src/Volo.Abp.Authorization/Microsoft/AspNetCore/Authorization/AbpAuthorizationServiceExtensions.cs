using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Authorization;
using Volo.Abp.Authorization.Fluent;

namespace Microsoft.AspNetCore.Authorization;

public static class AbpAuthorizationServiceExtensions
{
    public static async Task<AuthorizationResult> AuthorizeAsync(this IAuthorizationService authorizationService, string policyName)
    {
        return await AuthorizeAsync(
            authorizationService,
            null,
            policyName
        );
    }

    public static async Task<AuthorizationResult> AuthorizeAsync(this IAuthorizationService authorizationService, object resource, IAuthorizationRequirement requirement)
    {
        return await authorizationService.AuthorizeAsync(
            authorizationService.AsAbpAuthorizationService().CurrentPrincipal,
            resource,
            requirement
        );
    }

    public static async Task<AuthorizationResult> AuthorizeAsync(this IAuthorizationService authorizationService, object resource, AuthorizationPolicy policy)
    {
        return await authorizationService.AuthorizeAsync(
            authorizationService.AsAbpAuthorizationService().CurrentPrincipal,
            resource,
            policy
        );
    }

    public static async Task<AuthorizationResult> AuthorizeAsync(this IAuthorizationService authorizationService, AuthorizationPolicy policy)
    {
        return await AuthorizeAsync(
            authorizationService,
            null,
            policy
        );
    }

    public static async Task<AuthorizationResult> AuthorizeAsync(this IAuthorizationService authorizationService, object resource, IEnumerable<IAuthorizationRequirement> requirements)
    {
        return await authorizationService.AuthorizeAsync(
            authorizationService.AsAbpAuthorizationService().CurrentPrincipal,
            resource,
            requirements
        );
    }

    public static async Task<AuthorizationResult> AuthorizeAsync(this IAuthorizationService authorizationService, object resource, string policyName)
    {
        return await authorizationService.AuthorizeAsync(
            authorizationService.AsAbpAuthorizationService().CurrentPrincipal,
            resource,
            policyName
        );
    }

    public static async Task<bool> IsGrantedAsync(this IAuthorizationService authorizationService, string policyName)
    {
        return (await authorizationService.AuthorizeAsync(policyName)).Succeeded;
    }

    public static async Task<bool> IsGrantedAnyAsync(
        this IAuthorizationService authorizationService,
        params string[] policyNames)
    {
        Check.NotNullOrEmpty(policyNames, nameof(policyNames));

        foreach (var policyName in policyNames)
        {
            if ((await authorizationService.AuthorizeAsync(policyName)).Succeeded)
            {
                return true;
            }
        }

        return false;
    }

    public static async Task<bool> IsGrantedAsync(this IAuthorizationService authorizationService, object resource, IAuthorizationRequirement requirement)
    {
        return (await authorizationService.AuthorizeAsync(resource, requirement)).Succeeded;
    }

    public static async Task<bool> IsGrantedAsync(this IAuthorizationService authorizationService, object resource, AuthorizationPolicy policy)
    {
        return (await authorizationService.AuthorizeAsync(resource, policy)).Succeeded;
    }

    public static async Task<bool> IsGrantedAsync(this IAuthorizationService authorizationService, AuthorizationPolicy policy)
    {
        return (await authorizationService.AuthorizeAsync(policy)).Succeeded;
    }

    public static async Task<bool> IsGrantedAsync(this IAuthorizationService authorizationService, object resource, IEnumerable<IAuthorizationRequirement> requirements)
    {
        return (await authorizationService.AuthorizeAsync(resource, requirements)).Succeeded;
    }

    public static async Task<bool> IsGrantedAsync(this IAuthorizationService authorizationService, object resource, string policyName)
    {
        return (await authorizationService.AuthorizeAsync(resource, policyName)).Succeeded;
    }

    public static async Task<bool> IsGrantedAsync(this IAuthorizationService authorizationService,
        Action<AbpInitialFluentAuthorizationBuilder> fluent)
    {
        var builder = new AbpInitialFluentAuthorizationBuilder(new AbpFluentAuthorizationNodeModel(false, null));

        fluent(builder);

        var model = builder.Model;

        return await authorizationService.IsGrantedAsync(model);
    }

    private async static Task<bool> IsGrantedAsync(this IAuthorizationService authorizationService,
        AbpFluentAuthorizationNodeModel model)
    {
        var success = !model.IsNegation;

        foreach (var branch in model.GetBranches())
        {
            var isGranted = branch switch {
                AbpFluentAuthorizationResourceModel resourceModel =>
                    await resourceModel.IsGrantedAsync(authorizationService),
                AbpFluentAuthorizationConditionModel conditionModel =>
                    await conditionModel.ConditionFunc(),
                AbpFluentAuthorizationNodeModel nodeModel =>
                    await authorizationService.IsGrantedAsync(nodeModel),
                _ => throw new AbpException("Unknown fluent authorization model type.")
            };

            if (!isGranted && !model.IsOrNode)
            {
                return !success;
            }

            if (isGranted && model.IsOrNode)
            {
                return success;
            }
        }

        return model.IsOrNode ? !success : success;
    }

    public static async Task CheckAsync(this IAuthorizationService authorizationService, string policyName)
    {
        if (!await authorizationService.IsGrantedAsync(policyName))
        {
            throw new AbpAuthorizationException(code: AbpAuthorizationErrorCodes.GivenPolicyHasNotGrantedWithPolicyName)
                .WithData("PolicyName", policyName);
        }
    }

    public static async Task CheckAsync(this IAuthorizationService authorizationService, object resource, IAuthorizationRequirement requirement)
    {
        if (!await authorizationService.IsGrantedAsync(resource, requirement))
        {
            throw new AbpAuthorizationException(code: AbpAuthorizationErrorCodes.GivenRequirementHasNotGrantedForGivenResource)
                .WithData("ResourceName", resource);
        }
    }

    public static async Task CheckAsync(this IAuthorizationService authorizationService, object resource, AuthorizationPolicy policy)
    {
        if (!await authorizationService.IsGrantedAsync(resource, policy))
        {
            throw new AbpAuthorizationException(code: AbpAuthorizationErrorCodes.GivenPolicyHasNotGrantedForGivenResource)
                .WithData("ResourceName", resource);
        }
    }

    public static async Task CheckAsync(this IAuthorizationService authorizationService, AuthorizationPolicy policy)
    {
        if (!await authorizationService.IsGrantedAsync(policy))
        {
            throw new AbpAuthorizationException(code: AbpAuthorizationErrorCodes.GivenPolicyHasNotGranted);
        }
    }

    public static async Task CheckAsync(this IAuthorizationService authorizationService, object resource, IEnumerable<IAuthorizationRequirement> requirements)
    {
        if (!await authorizationService.IsGrantedAsync(resource, requirements))
        {
            throw new AbpAuthorizationException(code: AbpAuthorizationErrorCodes.GivenRequirementsHasNotGrantedForGivenResource)
                .WithData("ResourceName", resource);
        }
    }

    public static async Task CheckAsync(this IAuthorizationService authorizationService, object resource, string policyName)
    {
        if (!await authorizationService.IsGrantedAsync(resource, policyName))
        {
            throw new AbpAuthorizationException(code: AbpAuthorizationErrorCodes.GivenPolicyHasNotGrantedForGivenResource)
                .WithData("ResourceName", resource);
        }
    }

    public static async Task CheckAsync(this IAuthorizationService authorizationService,
        Action<AbpInitialFluentAuthorizationBuilder> fluent)
    {
        var builder = new AbpInitialFluentAuthorizationBuilder(new AbpFluentAuthorizationNodeModel(false, null));

        fluent(builder);

        var model = builder.Model;

        var exception = await authorizationService.CheckAsync(model);

        if (exception != null)
        {
            throw exception;
        }
    }

    [ItemCanBeNull]
    private async static Task<Exception> CheckAsync(this IAuthorizationService authorizationService,
        AbpFluentAuthorizationNodeModel model)
    {
        Exception Success() => model.IsNegation
            ? model.ExceptionForFailure ?? new AbpAuthorizationException() // todo: needs a code?
            : null;
        Exception Failure([NotNull] Exception exception) => model.IsNegation
            ? null
            : model.ExceptionForFailure ?? exception;

        foreach (var branch in model.GetBranches())
        {
            Exception exception = null;
            try
            {
                switch (branch)
                {
                    case AbpFluentAuthorizationResourceModel resourceModel:
                        await resourceModel.CheckAsync(authorizationService);
                        break;
                    case AbpFluentAuthorizationConditionModel conditionModel:
                        if (!await conditionModel.ConditionFunc())
                        {
                            throw new AbpAuthorizationException(); // todo: needs a code?
                        }
                        break;
                    case AbpFluentAuthorizationNodeModel nodeModel:
                        exception = await authorizationService.CheckAsync(nodeModel);
                        break;
                    default:
                        throw new AbpException("Unknown fluent authorization model type.");
                }
            }
            catch (Exception e)
            {
                exception = e;
            }

            if (exception != null && !model.IsOrNode)
            {
                return Failure(model.ExceptionForFailure ?? exception);
            }

            if (exception == null && model.IsOrNode)
            {
                return Success();
            }
        }

        if (model.IsOrNode)
        {
            return Failure(model.ExceptionForFailure ?? new AbpAuthorizationException()); // todo: needs a code?
        }

        return Success();
    }

    private static IAbpAuthorizationService AsAbpAuthorizationService(this IAuthorizationService authorizationService)
    {
        if (!(authorizationService is IAbpAuthorizationService abpAuthorizationService))
        {
            throw new AbpException($"{nameof(authorizationService)} should implement {typeof(IAbpAuthorizationService).FullName}");
        }

        return abpAuthorizationService;
    }
}
