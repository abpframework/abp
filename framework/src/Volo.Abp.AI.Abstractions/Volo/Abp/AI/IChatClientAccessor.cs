using Microsoft.Extensions.AI;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AI;

public interface IChatClientAccessor
{
    IChatClient? ChatClient { get; }
}

public interface IChatClientAccessor<TWorkSpace> : IChatClientAccessor
    where TWorkSpace : class
{
}