using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.Threading;

namespace Microsoft.Extensions.Hosting;

public static class AbpHostExtensions
{
    public static async Task InitializeAsync(this IHost host)
    {
        var application = host.Services.GetRequiredService<IAbpApplicationWithExternalServiceProvider>();
        var applicationLifetime = host.Services.GetRequiredService<IHostApplicationLifetime>();

        applicationLifetime.ApplicationStopping.Register(() => AsyncHelper.RunSync(() => application.ShutdownAsync()));
        applicationLifetime.ApplicationStopped.Register(() => application.Dispose());

        await application.InitializeAsync(host.Services);
    }
}
