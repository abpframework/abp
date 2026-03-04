using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace DistDemoApp;

public static class DistDemoEntityFrameworkServiceCollectionExtensions
{
    public static void ConfigureDistDemoEntityFrameworkInfrastructure(this ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<TodoDbContext>(options =>
        {
            options.AddDefaultRepositories();
        });

        context.Services.Configure<AbpDbContextOptions>(options =>
        {
            options.UseSqlServer();
        });
    }
}
