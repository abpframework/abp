using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;

namespace Microsoft.Extensions.Hosting
{
    public static class AbpHostExtensions
    {
        public static Task AbpRunAsync([NotNull] this IHost host, CancellationToken cancellationToken = default)
        {
            Check.NotNull(host, nameof(host));

            host.InitializeApplication();

            return host.RunAsync(cancellationToken);
        }

        public static Task AbpRunAsync([NotNull] this IHostBuilder builder, CancellationToken cancellationToken = default)
        {
            Check.NotNull(builder, nameof(builder));

            var host = builder.Build();

            host.InitializeApplication();

            return host.RunAsync(cancellationToken);
        }

        public static Task AbpRunConsoleAsync([NotNull] this IHostBuilder builder, CancellationToken cancellationToken = default)
        {
            Check.NotNull(builder, nameof(builder));

            var host = builder.UseConsoleLifetime().Build();

            host.InitializeApplication();

            return host.RunAsync(cancellationToken);
        }

        public static IHost InitializeApplication([NotNull] this IHost host)
        {
            Check.NotNull(host, nameof(host));

            var application = host.Services.GetRequiredService<IAbpApplicationWithExternalServiceProvider>();
            var applicationLifetime = host.Services.GetRequiredService<IHostApplicationLifetime>();

            applicationLifetime.ApplicationStopping.Register(() =>
            {
                application.Shutdown();
            });

            applicationLifetime.ApplicationStopped.Register(() =>
            {
                application.Dispose();
            });

            application.Initialize(host.Services);

            return host;
        }
    }
}
