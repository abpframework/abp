namespace Volo.Abp.Authorization.Fluent;

public interface IAbpRootNodeFluentAuthorizationBuilder<out TNextAndNode, out TNextOrNode> :
    IAbpAndNodeFluentAuthorizationBuilder<TNextAndNode>,
    IAbpOrNodeFluentAuthorizationBuilder<TNextOrNode>
    where TNextAndNode : IAbpFluentAuthorizationBuilder
    where TNextOrNode : IAbpFluentAuthorizationBuilder
{
}