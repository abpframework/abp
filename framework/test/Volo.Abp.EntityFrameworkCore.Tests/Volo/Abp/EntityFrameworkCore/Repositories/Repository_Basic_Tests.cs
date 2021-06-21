using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using Volo.Abp.TestApp.Testing;
using Xunit;

namespace Volo.Abp.EntityFrameworkCore.Repositories
{
    public class Repository_Basic_Tests : Repository_Basic_Tests<AbpEntityFrameworkCoreTestModule>
    {
        [Fact]
        public async Task EFCore_QueryableExtension_ToListAsync()
        {
            await WithUnitOfWorkAsync(async () =>
            {
                var persons = await PersonRepository.ToListAsync();
                persons.Count.ShouldBeGreaterThan(0);
            });
        }

        [Fact]
        public async Task EFCore_QueryableExtension_CountAsync()
        {
            await WithUnitOfWorkAsync(async () =>
            {
                var count = await PersonRepository.CountAsync();
                count.ShouldBeGreaterThan(0);
            });
        }
    }
}
