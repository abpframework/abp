using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;

namespace Volo.Abp.Authorization.Fluent;

public class AbpFluentAuthorizationResourceModel : IAbpFluentAuthorizationModel
{
    [CanBeNull]
    public object Resource { get; }

    [CanBeNull]
    public IAuthorizationRequirement Requirement { get; }

    [CanBeNull]
    public IEnumerable<IAuthorizationRequirement> Requirements { get; }

    [CanBeNull]
    public AuthorizationPolicy Policy { get; }

    [CanBeNull]
    public string PolicyName { get; }

    public AbpFluentAuthorizationResourceModel(object resource, string policyName)
    {
        Resource = resource;
        PolicyName = policyName;
    }

    public AbpFluentAuthorizationResourceModel(string policyName)
    {
        PolicyName = policyName;
    }

    public AbpFluentAuthorizationResourceModel(object resource, IAuthorizationRequirement requirement)
    {
        Resource = resource;
        Requirement = requirement;
    }

    public AbpFluentAuthorizationResourceModel(object resource, AuthorizationPolicy policy)
    {
        Resource = resource;
        Policy = policy;
    }

    public AbpFluentAuthorizationResourceModel(AuthorizationPolicy policy)
    {
        Policy = policy;
    }

    public AbpFluentAuthorizationResourceModel(object resource, IEnumerable<IAuthorizationRequirement> requirements)
    {
        Resource = resource;
        Requirements = requirements;
    }

    public async Task CheckAsync(IAuthorizationService authorizationService)
    {
        if (Resource != null && PolicyName != null)
        {
            await authorizationService.CheckAsync(Resource, PolicyName);
        }
        else if (PolicyName != null)
        {
            await authorizationService.CheckAsync(PolicyName);
        }
        else if (Requirement != null)
        {
            await authorizationService.CheckAsync(Resource, Requirement);
        }
        else if (Resource != null && Policy != null)
        {
            await authorizationService.CheckAsync(Resource, Policy);
        }
        else if (Policy != null)
        {
            await authorizationService.CheckAsync(Policy);
        }
        else if (Requirements != null)
        {
            await authorizationService.CheckAsync(Resource, Requirements);
        }
        else
        {
            throw new AbpException("Unknown authorization requirement type.");
        }
    }

    public async Task<bool> IsGrantedAsync(IAuthorizationService authorizationService)
    {
        if (Resource != null && PolicyName != null)
        {
            return await authorizationService.IsGrantedAsync(Resource, PolicyName);
        }

        if (PolicyName != null)
        {
            return await authorizationService.IsGrantedAsync(PolicyName);
        }

        if (Requirement != null)
        {
            return await authorizationService.IsGrantedAsync(Resource, Requirement);
        }

        if (Resource != null && Policy != null)
        {
            return await authorizationService.IsGrantedAsync(Resource, Policy);
        }

        if (Policy != null)
        {
            return await authorizationService.IsGrantedAsync(Policy);
        }

        if (Requirements != null)
        {
            return await authorizationService.IsGrantedAsync(Resource, Requirements);
        }

        throw new AbpException("Unknown authorization requirement type.");
    }
}