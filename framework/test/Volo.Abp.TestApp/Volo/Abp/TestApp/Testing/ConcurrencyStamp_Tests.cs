using System.Threading.Tasks;
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
            var london = await CityRepository.FindByNameAsync("London");
            london.Name = "London-1";
            await CityRepository.UpdateAsync(london);
        }
    }
}
