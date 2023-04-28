using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Volo.Abp.Authorization.Fluent;

public class AbpInitialFluentAuthorizationBuilder : AbpFluentAuthorizationBuilderBase,
    IAbpInitialFluentAuthorizationBuilder<AbpRootNodeFluentAuthorizationBuilder>
{
    public AbpInitialFluentAuthorizationBuilder(AbpFluentAuthorizationNodeModel model) : base(model)
    {
    }

    public AbpRootNodeFluentAuthorizationBuilder Meet(Action<AbpInitialFluentAuthorizationBuilder> config,
        Exception exceptionForFailure = null)
    {
        CreateAndBranch(config, false, exceptionForFailure);

        return new AbpRootNodeFluentAuthorizationBuilder(Model);
    }

    public AbpRootNodeFluentAuthorizationBuilder NotMeet(Action<AbpInitialFluentAuthorizationBuilder> config,
        Exception exceptionForFailure = null)
    {
        CreateAndBranch(config, true, exceptionForFailure);

        return new AbpRootNodeFluentAuthorizationBuilder(Model);
    }

    public void IsGranted(object resource, IAuthorizationRequirement requirement)
    {
        Model.AddAndBranch(new AbpFluentAuthorizationResourceModel(resource, requirement));
    }

    public void IsGranted(object resource, AuthorizationPolicy policy)
    {
        Model.AddAndBranch(new AbpFluentAuthorizationResourceModel(resource, policy));
    }

    public void IsGranted(AuthorizationPolicy policy)
    {
        Model.AddAndBranch(new AbpFluentAuthorizationResourceModel(policy));
    }

    public void IsGranted(object resource, IEnumerable<IAuthorizationRequirement> requirements)
    {
        Model.AddAndBranch(new AbpFluentAuthorizationResourceModel(resource, requirements));
    }

    public void IsGranted(object resource, string policyName)
    {
        Model.AddAndBranch(new AbpFluentAuthorizationResourceModel(resource, policyName));
    }

    public void Condition(Func<bool> condition, Exception exceptionForFailure = null)
    {
        Model.AddAndBranch(new AbpFluentAuthorizationConditionModel(condition, exceptionForFailure));
    }

    public void Condition(Func<Task<bool>> condition, Exception exceptionForFailure = null)
    {
        Model.AddAndBranch(new AbpFluentAuthorizationConditionModel(condition, exceptionForFailure));
    }
}