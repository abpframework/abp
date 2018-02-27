using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.AspNetCore.Modularity;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Autofac;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.Modularity;
using Volo.Abp.Permissions;
using Volo.Abp.Permissions.EntityFrameworkCore;

namespace MicroserviceDemo.PermissionService
{
    [DependsOn(typeof(AbpAutofacModule))]
    [DependsOn(typeof(AbpPermissionsApplicationModule))]
    [DependsOn(typeof(AbpPermissionsEntityFrameworkCoreModule))]
    [DependsOn(typeof(AbpAspNetCoreMvcModule))]
    [DependsOn(typeof(AbpIdentityEntityFrameworkCoreModule))]
    public class MicroservicesDemoPermissionServiceModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            var hostingEnvironment = services.GetSingletonInstance<IHostingEnvironment>();
            var configuration = BuildConfiguration(hostingEnvironment);

            services.Configure<DbConnectionOptions>(configuration);

            services.Configure<AbpDbContextOptions>(options =>
            {
                options.Configure(context =>
                {
                    if (context.ExistingConnection != null)
                    {
                        context.DbContextOptions.UseSqlServer(context.ExistingConnection);
                    }
                    else
                    {
                        context.DbContextOptions.UseSqlServer(context.ConnectionString);
                    }
                });

                //TODO: This should not be neededn when we fix the conn string name problem for interfaces
                options.Configure<IdentityDbContext>(context =>
                {
                    context.DbContextOptions.UseSqlServer(configuration.GetConnectionString("AbpIdentity"));
                });
            });

            services.Configure<AbpAspNetCoreMvcOptions>(options => //TODO: Will be moved to the AbpPermissionsHttpApiModule when it's available!
            {
                options.ConventionalControllers.Create(
                    typeof(AbpPermissionsApplicationModule).Assembly,
                    opts =>
                    {
                        opts.RootPath = "permission";
                    }
                );
            });

            //services.AddSwaggerGen(
            //    options =>
            //    {
            //        options.SwaggerDoc("v1", new Info { Title = "Permissions API", Version = "v1" });
            //        options.DocInclusionPredicate((docName, description) => true);
            //    });

            services.AddAlwaysAllowPermissionChecker(); //TODO: Remove when add authentication!

            services.AddAssemblyOf<MicroservicesDemoPermissionServiceModule>();
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();

            app.UseStaticFiles();

            //app.UseSwagger();
            //app.UseSwaggerUI(options =>
            //{
            //    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Permissions API");
            //});
            
            app.UseMvcWithDefaultRoute();
        }

        private static IConfigurationRoot BuildConfiguration(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);

            return builder.Build();
        }
    }
}