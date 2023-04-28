using System;
using JetBrains.Annotations;

namespace Volo.Abp.Authorization.Fluent;

public abstract class AbpFluentAuthorizationBuilderBase : IAbpFluentAuthorizationBuilder
{
    public AbpFluentAuthorizationNodeModel Model { get; }

    public AbpFluentAuthorizationBuilderBase(AbpFluentAuthorizationNodeModel model)
    {
        Model = model;
    }

    protected void CreateAndBranch(Action<AbpInitialFluentAuthorizationBuilder> config, bool isNegation,
        [CanBeNull] Exception exceptionForFailure)
    {
        var builder =
            new AbpInitialFluentAuthorizationBuilder(
                new AbpFluentAuthorizationNodeModel(isNegation, exceptionForFailure));

        config(builder);

        Model.AddAndBranch(builder.Model);
    }

    protected void CreateOrBranch(Action<AbpInitialFluentAuthorizationBuilder> config, bool isNegation,
        [CanBeNull] Exception exceptionForFailure)
    {
        var builder =
            new AbpInitialFluentAuthorizationBuilder(
                new AbpFluentAuthorizationNodeModel(isNegation, exceptionForFailure));

        config(builder);

        Model.AddOrBranch(builder.Model);
    }
}