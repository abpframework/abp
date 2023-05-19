using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Volo.Abp.AspNetCore.TestBase;

public class AbpNoopHostLifetime : IHostLifetime
{
    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task WaitForStartAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
