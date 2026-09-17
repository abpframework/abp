using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Components.Web.Configuration;

public class NullCurrentApplicationConfigurationCacheResetService : ICurrentApplicationConfigurationCacheResetService, ISingletonDependency
{
    public Task ResetAsync(Guid? userId = null)
    {
        return Task.CompletedTask;
    }
}
