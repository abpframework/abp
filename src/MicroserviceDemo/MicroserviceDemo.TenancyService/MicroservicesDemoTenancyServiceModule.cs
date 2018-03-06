using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using Volo.Abp;
using Volo.Abp.AspNetCore.Modularity;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Autofac;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.MultiTenancy.EntityFrameworkCore;
using Volo.Abp.Permissions.EntityFrameworkCore;
using Volo.Abp.Security.Claims;
using Volo.Abp.Threading;

namespace MicroserviceDemo.TenancyService
{
    [DependsOn(typeof(AbpAutofacModule))]
    [DependsOn(typeof(AbpMultiTenancyEntityFrameworkCoreModule))]
    [DependsOn(typeof(AbpMultiTenancyHttpApiModule))]
    [DependsOn(typeof(AbpMultiTenancyApplicationModule))]
    [DependsOn(typeof(AbpPermissionsEntityFrameworkCoreModule))]
    public class MicroservicesDemoTenancyServiceModule : AbpModule
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
            });

            services.AddSwaggerGen(
                options =>
                {
                    options.SwaggerDoc("v1", new Info { Title = "Multi-Tenancy API", Version = "v1" });
                    options.DocInclusionPredicate((docName, description) => true);
                });

            services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = "http://localhost:54307";
                    options.RequireHttpsMetadata = false;

                    options.ApiName = "multi-tenancy-api";

                    //This should be automatically done :(
                    options.InboundJwtClaimTypeMap["sub"] = AbpClaimTypes.UserId;
                    options.InboundJwtClaimTypeMap["role"] = AbpClaimTypes.Role;
                    options.InboundJwtClaimTypeMap["email"] = AbpClaimTypes.Email;
                });

            services.AddDistributedSqlServerCache(options =>
            {
                options.ConnectionString = configuration.GetConnectionString("SqlServerCache");
                options.SchemaName = "dbo";
                options.TableName = "TestCache";
            });

            services.AddAssemblyOf<MicroservicesDemoTenancyServiceModule>();
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();

            var store = context.ServiceProvider.GetRequiredService<IPermissionStore>();
            var xx = AsyncHelper.RunSync(() => store.IsGrantedAsync("AbpTenantManagement.Tenants", "Role", "admin"));

            app.UseStaticFiles();

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Multi-Tenancy API");
            });

            app.UseAuthentication();

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