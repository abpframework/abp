namespace Volo.Abp.Authorization.Fluent;

public interface IAbpInitialFluentAuthorizationBuilder<out TNextAndNode> :
    IFluentAuthorizationConditions,
    IAbpAndNodeFluentAuthorizationBuilder<TNextAndNode>
    where TNextAndNode : IAbpFluentAuthorizationBuilder
{
}