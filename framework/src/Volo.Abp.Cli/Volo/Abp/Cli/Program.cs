using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Threading;

namespace Volo.Abp.Cli
{
    public class Program
    {
        private static void Main(string[] args)
        {
            using (var application = AbpApplicationFactory.Create<AbpCliModule>())
            {
                application.Initialize();

                AsyncHelper.RunSync(
                    () => application.ServiceProvider
                        .GetRequiredService<CliService>()
                        .RunAsync(args)
                );

                application.Shutdown();
            }
        }
    }
}
