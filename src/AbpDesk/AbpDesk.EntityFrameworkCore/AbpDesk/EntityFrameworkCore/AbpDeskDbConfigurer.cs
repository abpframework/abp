using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace AbpDesk.EntityFrameworkCore
{
    public static class AbpDeskDbConfigurer
    {
        public static void Configure(IServiceCollection services, IConfigurationRoot configuration)
        {
            //Configure DbConnectionOptions by configuration file (appsettings.json)
            services.Configure<DbConnectionOptions>(configuration); //TODO: Move to the application. No proper to be in EF Core package.

            services.Configure<AbpDbContextOptions>(options =>
            {
                //Configures all dbcontextes to use Sql Server with calculated connection string
                options.Configure(context =>
                {
                    //TODO: Create an extension method to AbpDbContextConfigurationContext to use SqlServer in single line call! Example:
                    /* context.UseSqlServer(opional action to configure sqlserver) 
                     * This internally makes the if below!
                     */
                    if (context.ExistingConnection != null)
                    {
                        context.DbContextOptions.UseSqlServer(context.ExistingConnection);
                    }
                    else
                    {
                        context.DbContextOptions.UseSqlServer(context.ConnectionString);
                    }
                });
            });
        }
    }
}