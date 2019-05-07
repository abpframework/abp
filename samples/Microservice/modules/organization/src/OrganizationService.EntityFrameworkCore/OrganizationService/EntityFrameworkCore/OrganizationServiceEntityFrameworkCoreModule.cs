using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace OrganizationService.EntityFrameworkCore
{
    [DependsOn(
        typeof(OrganizationServiceDomainModule),
        typeof(AbpEntityFrameworkCoreModule)
    )]
    public class OrganizationServiceEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<OrganizationServiceDbContext>(options =>
            {
                options.AddDefaultRepositories<IOrganizationServiceDbContext>(true);

                options.AddRepository<Organization, EfCoreOrganizationRepository>();

            });

       
        }
    }
}