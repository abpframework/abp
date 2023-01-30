using Microsoft.EntityFrameworkCore;
using Volo.Abp.DependencyInjection;

namespace MyCompanyName.MyProjectName.Data;

public class MyProjectNameEFCoreDbSchemaMigrator : ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public MyProjectNameEFCoreDbSchemaMigrator(
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolving the MyProjectNameDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<MyProjectNameDbContext>()
            .Database
            .MigrateAsync();
    }
}
