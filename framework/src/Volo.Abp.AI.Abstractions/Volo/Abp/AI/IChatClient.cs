using Microsoft.Extensions.AI;

namespace Volo.Abp.AI;

public interface IChatClient<TWorkSpace> : IChatClient
    where TWorkSpace : class
{
    
}