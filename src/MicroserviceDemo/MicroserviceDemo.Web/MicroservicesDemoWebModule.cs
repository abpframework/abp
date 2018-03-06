using System;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Swashbuckle.AspNetCore.Swagger;
using Volo.Abp;
using Volo.Abp.AspNetCore.Authentication.OAuth;
using Volo.Abp.AspNetCore.Modularity;
using Volo.Abp.AspNetCore.Mvc.Bundling;
using Volo.Abp.Autofac;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Http.Client;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.Identity.Web;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.MultiTenancy.Web;
using Volo.Abp.Permissions.EntityFrameworkCore;

namespace MicroserviceDemo.Web
{
    [DependsOn(typeof(AbpAutofacModule))]
    [DependsOn(typeof(AbpPermissionsEntityFrameworkCoreModule))]
    [DependsOn(typeof(AbpIdentityHttpApiModule))]
    [DependsOn(typeof(AbpIdentityWebModule))]
    [DependsOn(typeof(AbpIdentityEntityFrameworkCoreModule))]
    [DependsOn(typeof(AbpMultiTenancyHttpApiClientModule))]
    [DependsOn(typeof(AbpMultiTenancyWebModule))]
    [DependsOn(typeof(AbpAspNetCoreAuthenticationOAuthModule))]
    public class MicroservicesDemoWebModule : AbpModule
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

            services.Configure<BundlingOptions>(options =>
            {
                //TODO: To the framework!
                options.ScriptBundles.Add("GlobalScripts", new[]
                {
                    "/Abp/ApplicationConfigurationScript?_v=" + DateTime.Now.Ticks,
                    "/Abp/ServiceProxyScript?_v=" + DateTime.Now.Ticks
                }); 
            });
            
            services.AddAuthentication(options =>
                {
                    //options.DefaultScheme = IdentityConstants.ApplicationScheme;
                    options.DefaultChallengeScheme = "oidc";
                })
                .AddOpenIdConnect("oidc", options =>
                {
                    options.SignInScheme = IdentityConstants.ApplicationScheme;

                    options.Authority = "http://localhost:54307";
                    options.RequireHttpsMetadata = false;

                    options.ClientId = "client";
                    options.ClientSecret = "secret";

                    options.ResponseType = OpenIdConnectResponseType.CodeIdToken;

                    options.SaveTokens = true;
                    options.GetClaimsFromUserInfoEndpoint = true;

                    options.Scope.Add("role");
                    options.Scope.Add("email");
                    options.Scope.Add("phone");
                    options.Scope.Add("multi-tenancy-api");

                    options.ClaimActions.MapAbpClaimTypes();
                });

            services.Configure<RemoteServiceOptions>(configuration);

            services.AddSwaggerGen(
                options =>
                {
                    options.SwaggerDoc("v1", new Info { Title = "Microservice Test Web API", Version = "v1" });
                    options.DocInclusionPredicate((docName, description) => true);
                });

            services.AddDistributedSqlServerCache(options =>
            {
                options.ConnectionString = configuration.GetConnectionString("SqlServerCache");
                options.SchemaName = "dbo";
                options.TableName = "TestCache";
            });

            services.AddAssemblyOf<MicroservicesDemoWebModule>();
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();

            if (context.GetEnvironment().IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseVirtualFiles();

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Microservice Test Web API");
            });

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "defaultWithArea",
                    template: "{area}/{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
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