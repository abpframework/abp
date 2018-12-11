using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.Modularity;
using Volo.Abp.TestApp.Domain;
using Xunit;

namespace Volo.Abp.TestApp.Testing
{
    public class ConcurrencyStamp_Tests<TStartupModule> : TestAppTestBase<TStartupModule>
        where TStartupModule : IAbpModule
    {
        protected readonly ICityRepository CityRepository;

        public ConcurrencyStamp_Tests()
        {
            CityRepository = GetRequiredService<ICityRepository>();
        }

        [Fact]
        public async Task Should_Not_Allow_To_Update_If_The_Entity_Has_Changed()
        {
            var london1 = await CityRepository.FindByNameAsync("London");
            london1.Name = "London-1";

            var london2 = await CityRepository.FindByNameAsync("London");
            london2.Name = "London-2";
            await CityRepository.UpdateAsync(london2);

            await Assert.ThrowsAsync<AbpDbConcurrencyException>(async () =>
            {
                await CityRepository.UpdateAsync(london1);
            });
        }
    }
}
