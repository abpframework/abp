using Microsoft.Extensions.AI;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AI;

[Dependency(TryRegister = true)]
[ExposeServices(typeof(IChatClientAccessor))]
public class NullChatClientAccessor : IChatClientAccessor
{
    public IChatClient? ChatClient => null;
}

[Dependency(TryRegister = true)]
[ExposeServices(typeof(IChatClientAccessor<>))]
public class NullChatClientAccessor<TWorkSpace> : IChatClientAccessor<TWorkSpace>
    where TWorkSpace : class
{
    public IChatClient? ChatClient => null;
}
