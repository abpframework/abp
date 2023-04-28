using System;

namespace Volo.Abp.Authorization.Fluent;

public class AbpRootNodeFluentAuthorizationBuilder : AbpFluentAuthorizationBuilderBase,
    IAbpRootNodeFluentAuthorizationBuilder<AbpAndNodeFluentAuthorizationBuilder, AbpOrNodeFluentAuthorizationBuilder>
{
    public AbpRootNodeFluentAuthorizationBuilder(AbpFluentAuthorizationNodeModel model) : base(model)
    {
    }

    public AbpAndNodeFluentAuthorizationBuilder Meet(Action<AbpInitialFluentAuthorizationBuilder> config,
        Exception exceptionForFailure = null)
    {
        CreateAndBranch(config, false, exceptionForFailure);

        return new AbpAndNodeFluentAuthorizationBuilder(Model);
    }

    public AbpAndNodeFluentAuthorizationBuilder NotMeet(Action<AbpInitialFluentAuthorizationBuilder> config,
        Exception exceptionForFailure = null)
    {
        CreateAndBranch(config, true, exceptionForFailure);

        return new AbpAndNodeFluentAuthorizationBuilder(Model);
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