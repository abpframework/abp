using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.Threading;

namespace Volo.Abp.EntityFrameworkCore.Design;

public abstract class AbpDesignTimeDbContextBase<TModule, TContext> : IDesignTimeDbContextFactory<TContext>
    where TModule : AbpModule
    where TContext : DbContext
{
    public virtual TContext CreateDbContext(string[] args)
    {
        return AsyncHelper.RunSync(() => CreateDbContextAsync(args));
    }

    protected virtual async Task<TContext> CreateDbContextAsync(string[] args)
    {
        var application = await AbpApplicationFactory.CreateAsync<TModule>(options =>
        {
            options.Services.ReplaceConfiguration(BuildConfiguration());
            ConfigureServices(options.Services);
        });

        await application.InitializeAsync();

        return application.ServiceProvider.GetRequiredService<TContext>();
    }

    protected virtual void ConfigureServices(IServiceCollection services)
    {

    }

    protected abstract IConfigurationRoot BuildConfiguration();
}
