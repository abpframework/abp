using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;
using Volo.Abp.AspNetCore.Modularity;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Autofac;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.Identity.HttpApi.Host.VersioningTests.V1;
using Volo.Abp.Modularity;

namespace Volo.Abp.Identity.HttpApi.Host
{
    [DependsOn(
        typeof(AbpIdentityApplicationModule),
        typeof(AbpIdentityEntityFrameworkCoreModule),
        typeof(AbpIdentityHttpApiModule), 
        typeof(AbpAutofacModule))]
    public class AbpIdentityHttpApiHostModule : AbpModule
    {
        public override void PreConfigureServices(IServiceCollection services)
        {
            services.PreConfigure<IMvcCoreBuilder>(builder =>
            {
                // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
                // note: the specified format code will format the version as "'v'major[.minor][-status]"

                builder.AddVersionedApiExplorer(o => o.GroupNameFormat = "'v'VVV");
            });
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            var hostingEnvironment = services.GetSingletonInstance<IHostingEnvironment>(); //TOD: Move to BuildConfiguration method
            var configuration = BuildConfiguration(hostingEnvironment);

            services.Configure<DbConnectionOptions>(configuration);

            services.Configure<AbpDbContextOptions>(options =>
            {
                //Configures all dbcontextes to use Sql Server with calculated connection string
                options.Configure(context =>
                {
                    context.DbContextOptions.UseSqlServer(context.ConnectionString);
                });
            });

            services.AddApiVersioning(o =>
            {
                o.ReportApiVersions = true;
                //o.Conventions.Controller<AbpApiDefinitionController>().Action((MethodInfo)null).MapToApiVersion(new ApiVersion(1,1),).IsApiVersionNeutral();
                //o.Conventions.Controller<AbpApiDefinitionController>().HasApiVersion(new ApiVersion(3, 0)); //We can do that based on controller's AbpApiVersion attribute!
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.DefaultApiVersion = new ApiVersion(3, 0); //Default: 1.0 //We can not rely on that, application should do.

                //o.ErrorResponses //TOD: We can override error response generator (to solve https://github.com/Microsoft/aspnet-api-versioning/issues/195)

                //o.Conventions.Controller<IdentityUserAppService>().HasApiVersion(2, 0);
                //o.Conventions.Controller<IdentityRoleAppService>().IsApiVersionNeutral();

                o.ConfigureAbp(services);
            });

            services.AddSwaggerGen(
                options =>
                {
                    //options.SwaggerDoc("v1", new Info { Title = "Volo.Abp.Identity API", Version = "v1" });
                    //options.DocInclusionPredicate((docName, description) => true);


                    // resolve the IApiVersionDescriptionProvider service
                    // note: that we have to build a temporary service provider here because one has not been created yet
                    var provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();

                    // add a swagger document for each discovered API version
                    // note: you might choose to skip or document deprecated API versions differently
                    foreach (var description in provider.ApiVersionDescriptions)
                    {
                        options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
                    }

                    // add a custom operation filter which sets default values
                    options.OperationFilter<SwaggerDefaultValues>();

                    // integrate xml comments
                    //options.IncludeXmlComments(XmlCommentsFilePath); //TODO: Add XML comments!
                });

            services.Configure<AbpAspNetCoreMvcOptions>(options =>
            {
                options.ConventionalControllers.Create(typeof(AbpIdentityHttpApiHostModule).Assembly, o =>
                {
                    o.TypePredicate = t => t == typeof(CallsController);
                });

                options.ConventionalControllers.Create(typeof(AbpIdentityHttpApiHostModule).Assembly, o =>
                {
                    o.TypePredicate = t => t == typeof(Host.VersioningTests.V2.CallsController);
                    o.RootPath = "app/compat";
                });
            });

            services.AddAssemblyOf<AbpIdentityHttpApiHostModule>();
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();

            context.GetLoggerFactory().AddConsole().AddDebug();

            if (context.GetEnvironment().IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            var provider = context.ServiceProvider.GetRequiredService<IApiVersionDescriptionProvider>();

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                //options.SwaggerEndpoint("/swagger/v1/swagger.json", "Volo.Abp.Identity API");



                // build a swagger endpoint for each discovered API version
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                }
            });

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

        private static Info CreateInfoForApiVersion(ApiVersionDescription description)
        {
            var info = new Info()
            {
                Title = $"Sample API {description.ApiVersion}",
                Version = description.ApiVersion.ToString(),
                Description = "A sample application with Swagger, Swashbuckle, and API versioning.",
                Contact = new Contact() { Name = "Bill Mei", Email = "bill.mei@somewhere.com" },
                TermsOfService = "Shareware",
                License = new License() { Name = "MIT", Url = "https://opensource.org/licenses/MIT" }
            };

            if (description.IsDeprecated)
            {
                info.Description += " This API version has been deprecated.";
            }

            return info;
        }
    }
}
