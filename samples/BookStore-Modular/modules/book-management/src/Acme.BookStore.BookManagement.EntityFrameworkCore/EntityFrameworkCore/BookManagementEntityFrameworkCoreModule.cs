using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Acme.BookStore.BookManagement.EntityFrameworkCore
{
    [DependsOn(
        typeof(BookManagementDomainModule),
        typeof(AbpEntityFrameworkCoreModule)
    )]
    public class BookManagementEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<BookManagementDbContext>(options =>
            {
                options.AddDefaultRepositories<IBookManagementDbContext>();
                /* Add custom repositories here. Example:
                 * options.AddRepository<Question, EfCoreQuestionRepository>();
                 */
            });
        }
    }
}