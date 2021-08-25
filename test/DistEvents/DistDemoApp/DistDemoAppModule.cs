using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Autofac;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.SqlServer;
using Volo.Abp.Modularity;

namespace DistDemoApp
{
    [DependsOn(
        typeof(AbpEntityFrameworkCoreSqlServerModule),
        typeof(AbpAutofacModule)
    )]
    public class DistDemoAppModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHostedService<MyProjectNameHostedService>();
            
            context.Services.AddAbpDbContext<TodoDbContext>(options =>
            {
                options.AddDefaultRepositories();
            });

            Configure<AbpDbContextOptions>(options =>
            {
                options.UseSqlServer();
            });
            
            Configure<AbpDistributedEntityEventOptions>(options =>
            {
                options.EtoMappings.Add<TodoItem, TodoItemEto>();
                options.AutoEventSelectors.Add<TodoItem>();
            });
        }
    }
}