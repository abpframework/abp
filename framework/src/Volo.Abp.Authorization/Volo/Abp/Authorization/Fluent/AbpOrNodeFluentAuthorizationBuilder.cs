using System;

namespace Volo.Abp.Authorization.Fluent;

public class AbpOrNodeFluentAuthorizationBuilder : AbpFluentAuthorizationBuilderBase,
    IAbpOrNodeFluentAuthorizationBuilder<AbpOrNodeFluentAuthorizationBuilder>
{
    public AbpOrNodeFluentAuthorizationBuilder(AbpFluentAuthorizationNodeModel model) : base(model)
    {
    }

    public AbpOrNodeFluentAuthorizationBuilder OrMeet(Action<AbpInitialFluentAuthorizationBuilder> config,
        Exception exceptionForFailure = null)
    {
        CreateOrBranch(config, false, exceptionForFailure);

        return new AbpOrNodeFluentAuthorizationBuilder(Model);
    }

    public AbpOrNodeFluentAuthorizationBuilder OrNotMeet(Action<AbpInitialFluentAuthorizationBuilder> config,
        Exception exceptionForFailure = null)
    {
        CreateOrBranch(config, true, exceptionForFailure);

        return new AbpOrNodeFluentAuthorizationBuilder(Model);
    }
}