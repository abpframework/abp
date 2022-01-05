using System;

namespace Volo.Abp.AspNetCore.SignalR;

public interface IAbpHubContextAccessor
{
    AbpHubContext Context { get; }

    IDisposable Change(AbpHubContext context);
}

