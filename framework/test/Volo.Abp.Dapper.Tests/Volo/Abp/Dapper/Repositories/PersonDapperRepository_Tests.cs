using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.TestApp;
using Volo.Abp.Uow;
using Xunit;

namespace Volo.Abp.Dapper.Repositories
{
    public class PersonDapperRepository_Tests : DapperTestBase
    {
        [Fact]
        public async Task GetAllPersonNames_Test()
        {
            var allNames = await GetRequiredService<PersonDapperRepository>().GetAllPersonNames();
            allNames.ShouldNotBeEmpty();
            allNames.ShouldContain(x => x == "Douglas");
            allNames.ShouldContain(x => x == "John-Deleted");
            allNames.ShouldContain(x => x == $"{TestDataBuilder.TenantId1}-Person1");
            allNames.ShouldContain(x => x == $"{TestDataBuilder.TenantId1}-Person2");
        }

        [Fact]
        public async Task UpdatePersonNames_Test()
        {
            var personDapperRepository = GetRequiredService<PersonDapperRepository>();
            await personDapperRepository.UpdatePersonNames("test");

            var allNames = await personDapperRepository.GetAllPersonNames();
            allNames.ShouldNotBeEmpty();
            allNames.ShouldAllBe(x => x == "test");
        }

        [Fact]
        public async Task Dapper_Transaction_Test()
        {
            var unitOfWorkManager = GetRequiredService<IUnitOfWorkManager>();
            var personDapperRepository = GetRequiredService<PersonDapperRepository>();

            using (var uow = unitOfWorkManager.Begin(new AbpUnitOfWorkOptions
            {
                IsTransactional = true
            }))
            {
                await personDapperRepository.UpdatePersonNames("test");
                await uow.RollbackAsync();
            }

            var allNames = await personDapperRepository.GetAllPersonNames();
            allNames.ShouldAllBe(x => x != "test");
        }
    }
}