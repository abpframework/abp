using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;

namespace MyCompanyName.MyProjectName.Data;

public class DataSeederMiddleware : IMiddleware, ISingletonDependency
{
    private bool _hostSeeded;

    private readonly ILogger<DataSeederMiddleware> _logger;
    private readonly IDataSeeder _dataSeeder;

    public DataSeederMiddleware(
        ILogger<DataSeederMiddleware> logger,
        IDataSeeder dataSeeder)
    {
        _logger = logger;
        _dataSeeder = dataSeeder;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        /* This logic is not safe if you are running multiple instances of your
         * application in parallel. In that case, a distributed lock usage is suggested,
         * or you can create another application for database migration/seed.
         */
        if (!_hostSeeded)
        {
            await SeedHostDataAsync();
            _hostSeeded = true;
        }

        await next(context);
    }

    private Task SeedHostDataAsync()
    {
        _logger.LogInformation($"Executing database seed...");

        return _dataSeeder.SeedAsync(new DataSeedContext()
            .WithProperty(IdentityDataSeedContributor.AdminEmailPropertyName,
                IdentityDataSeedContributor.AdminEmailDefaultValue)
            .WithProperty(IdentityDataSeedContributor.AdminPasswordPropertyName,
                IdentityDataSeedContributor.AdminPasswordDefaultValue)
        );
    }
}
