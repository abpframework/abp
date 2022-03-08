using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Volo.Abp.AspNetCore.SignalR.Uow;

public class AbpUowHubFilterOptions
{
    public Func<HubInvocationContext, Task<bool>> IsTransactional { get; set; }

    public AbpUowHubFilterOptions()
    {
        IsTransactional = context => Task.FromResult<bool>(true);
    }
}
