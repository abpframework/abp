using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Localization.Distributed;
using Volo.Abp.Localization.Resources.AbpLocalization;
using Volo.Abp.Modularity;
using Volo.Abp.Settings;
using Volo.Abp.Threading;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.Localization;

[DependsOn(
    typeof(AbpVirtualFileSystemModule),
    typeof(AbpSettingsModule),
    typeof(AbpLocalizationAbstractionsModule),
    typeof(AbpThreadingModule)
    )]
public class AbpLocalizationModule : AbpModule
{
    private readonly CancellationTokenSource _cancellationTokenSource = new();

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        AbpStringLocalizerFactory.Replace(context.Services);

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpLocalizationModule>("Volo.Abp", "Volo/Abp");
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options
                .Resources
                .Add<DefaultResource>("en");

            options
                .Resources
                .Add<AbpLocalizationResource>("en")
                .AddVirtualJson("/Localization/Resources/AbpLocalization");
        });
    }

    public async override Task OnApplicationInitializationAsync(ApplicationInitializationContext context)
    {
        await SaveLocalizationsAsync(context);
    }

    public override Task OnApplicationShutdownAsync(ApplicationShutdownContext context)
    {
        _cancellationTokenSource.Cancel();
        return Task.CompletedTask;
    }

    private async Task SaveLocalizationsAsync(ApplicationInitializationContext context)
    {
        var options = context
            .ServiceProvider
            .GetRequiredService<IOptions<AbpDistributedLocalizationOptions>>()
            .Value;

        if (!options.SaveToDistributedStore)
        {
            return;
        }   
        
        var rootServiceProvider = context.ServiceProvider.GetRequiredService<IRootServiceProvider>();

        Task.Run(async () =>
        {
            using var scope = rootServiceProvider.CreateScope();
            var applicationLifetime = scope.ServiceProvider.GetService<IHostApplicationLifetime>();
            var cancellationTokenProvider = scope.ServiceProvider.GetRequiredService<ICancellationTokenProvider>();
            var cancellationToken = applicationLifetime?.ApplicationStopping ?? _cancellationTokenSource.Token;
            
            try
            {
                using (cancellationTokenProvider.Use(cancellationToken))
                {
                    if (cancellationTokenProvider.Token.IsCancellationRequested)
                    {
                        return;
                    }
                    
                    await Policy
                        .Handle<Exception>()
                        .WaitAndRetryAsync(8, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt) * 10))
                        .ExecuteAsync(async _ =>
                        {
                            try
                            {
                                // ReSharper disable once AccessToDisposedClosure
                                await scope
                                    .ServiceProvider
                                    .GetRequiredService<IDistributedLocalizationStore>()
                                    .SaveAsync();
                            }
                            catch (Exception ex)
                            {
                                // ReSharper disable once AccessToDisposedClosure
                                scope.ServiceProvider
                                    .GetService<ILogger<AbpLocalizationModule>>()?
                                    .LogException(ex);

                                throw; // Polly will catch it
                            }
                        }, cancellationTokenProvider.Token);
                }
            }
            // ReSharper disable once EmptyGeneralCatchClause (No need to log since it is logged above)
            catch { }
        });
    }
}
