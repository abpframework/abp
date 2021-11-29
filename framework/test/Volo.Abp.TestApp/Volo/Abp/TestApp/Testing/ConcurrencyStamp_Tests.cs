using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.Modularity;
using Volo.Abp.TestApp.Domain;
using Xunit;

namespace Volo.Abp.TestApp.Testing;

public abstract class ConcurrencyStamp_Tests<TStartupModule> : TestAppTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    protected readonly ICityRepository CityRepository;

    protected ConcurrencyStamp_Tests()
    {
        CityRepository = GetRequiredService<ICityRepository>();
    }

    [Fact]
    public async Task Should_Not_Allow_To_Update_If_The_Entity_Has_Changed()
    {
        //Got an entity from database, changed its value, but not updated in the database yet
        var london1 = await CityRepository.FindByNameAsync("London");
        london1.Name = "London-1";

        //Another user has changed it just before I update
        var london2 = await CityRepository.FindByNameAsync("London");
        london2.Name = "London-2";
        await CityRepository.UpdateAsync(london2);

        //And updating my old entity throws exception!
        await Assert.ThrowsAsync<AbpDbConcurrencyException>(async () =>
        {
            await CityRepository.UpdateAsync(london1);
        });
    }

    [Fact]
    public async Task Should_Not_Allow_To_Delete_If_The_Entity_Has_Changed()
    {
        //Got an entity from database, but not deleted in the database yet
        var london1 = await CityRepository.FindByNameAsync("London");

        //Another user has changed it just before I delete
        var london2 = await CityRepository.FindByNameAsync("London");
        london2.Name = "London-updated";
        await CityRepository.UpdateAsync(london2);

        //And deleting my old entity throws exception!
        await Assert.ThrowsAsync<AbpDbConcurrencyException>(async () =>
        {
            await CityRepository.DeleteAsync(london1);
        });
    }
}
