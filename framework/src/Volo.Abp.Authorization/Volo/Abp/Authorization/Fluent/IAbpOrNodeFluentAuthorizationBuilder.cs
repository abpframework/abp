using System;
using JetBrains.Annotations;

namespace Volo.Abp.Authorization.Fluent;

public interface IAbpOrNodeFluentAuthorizationBuilder<out TNextNode> : IAbpFluentAuthorizationBuilder
    where TNextNode : IAbpFluentAuthorizationBuilder
{
    TNextNode OrMeet(Action<AbpInitialFluentAuthorizationBuilder> config,
        [CanBeNull] Exception exceptionForFailure = null);

    TNextNode OrNotMeet(Action<AbpInitialFluentAuthorizationBuilder> config,
        [CanBeNull] Exception exceptionForFailure = null);
}