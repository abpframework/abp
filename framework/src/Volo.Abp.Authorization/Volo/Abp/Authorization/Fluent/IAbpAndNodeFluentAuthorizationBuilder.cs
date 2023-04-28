using System;
using JetBrains.Annotations;

namespace Volo.Abp.Authorization.Fluent;

public interface IAbpAndNodeFluentAuthorizationBuilder<out TNextNode> : IAbpFluentAuthorizationBuilder
    where TNextNode : IAbpFluentAuthorizationBuilder
{
    TNextNode Meet(Action<AbpInitialFluentAuthorizationBuilder> config,
        [CanBeNull] Exception exceptionForFailure = null);

    TNextNode NotMeet(Action<AbpInitialFluentAuthorizationBuilder> config,
        [CanBeNull] Exception exceptionForFailure = null);
}