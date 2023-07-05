using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;

namespace Volo.Abp.Data;

//TODO: Create a Volo.Abp.Data.Seeding namespace?
public class DataSeeder : IDataSeeder, ITransientDependency
{
    protected IServiceScopeFactory ServiceScopeFactory { get; }
    protected AbpDataSeedOptions Options { get; }

    public DataSeeder(
        IOptions<AbpDataSeedOptions> options,
        IServiceScopeFactory serviceScopeFactory)
    {
        ServiceScopeFactory = serviceScopeFactory;
        Options = options.Value;
    }

    [UnitOfWork]
    public virtual async Task SeedAsync(DataSeedContext context)
    {
        using (var scope = ServiceScopeFactory.CreateScope())
        {
            if (context.Properties.ContainsKey(DataSeederExtensions.SeedInSeparateUow))
            {
                var manager = scope.ServiceProvider.GetRequiredService<IUnitOfWorkManager>();
                foreach (var contributorType in Options.Contributors)
                {
                    var options = context.Properties.TryGetValue(DataSeederExtensions.SeedInSeparateUowOptions, out var uowOptions)
                        ? (AbpUnitOfWorkOptions) uowOptions!
                        : new AbpUnitOfWorkOptions();
                    var requiresNew = context.Properties.TryGetValue(DataSeederExtensions.SeedInSeparateUowRequiresNew, out var obj) && (bool) obj!;

                    using (var uow = manager.Begin(options, requiresNew))
                    {
                        var contributor = (IDataSeedContributor)scope.ServiceProvider.GetRequiredService(contributorType);
                        await contributor.SeedAsync(context);
                        await uow.CompleteAsync();
                    }
                }
            }
            else
            {
                foreach (var contributorType in Options.Contributors)
                {
                    var contributor = (IDataSeedContributor)scope.ServiceProvider.GetRequiredService(contributorType);
                    await contributor.SeedAsync(context);
                }
            }
        }
    }
}
