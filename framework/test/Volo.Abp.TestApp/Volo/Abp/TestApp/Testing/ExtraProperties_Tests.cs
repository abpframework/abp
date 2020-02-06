using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Data;
using Volo.Abp.Modularity;
using Volo.Abp.TestApp.Domain;
using Xunit;

namespace Volo.Abp.TestApp.Testing
{
    public abstract class ExtraProperties_Tests<TStartupModule> : TestAppTestBase<TStartupModule>
        where TStartupModule : IAbpModule
    {
        protected readonly ICityRepository CityRepository;

        protected ExtraProperties_Tests()
        {
            CityRepository = GetRequiredService<ICityRepository>();
        }

        [Fact]
        public async Task Should_Get_An_Extra_Property()
        {
            var london = await CityRepository.FindByNameAsync("London");
            london.HasProperty("Population").ShouldBeTrue();
            london.GetProperty<int>("Population").ShouldBe(10_470_000);
        }

        [Fact]
        public async Task Should_Add_An_Extra_Property()
        {
            var london = await CityRepository.FindByNameAsync("London");
            london.SetProperty("AreaAsKm", 1572);
            await CityRepository.UpdateAsync(london);

            var london2 = await CityRepository.FindByNameAsync("London");
            london2.HasProperty("AreaAsKm").ShouldBeTrue();
            london2.GetProperty<int>("AreaAsKm").ShouldBe(1572);
        }

        [Fact]
        public async Task Should_Update_An_Existing_Extra_Property()
        {
            var london = await CityRepository.FindByNameAsync("London");

            london.ExtraProperties["Population"] = 11_000_042;
            await CityRepository.UpdateAsync(london);

            var london2 = await CityRepository.FindByNameAsync("London");
            london2.HasProperty("Population").ShouldBeTrue();
            london2.GetProperty<int>("Population").ShouldBe(11_000_042);
        }
    }
}
