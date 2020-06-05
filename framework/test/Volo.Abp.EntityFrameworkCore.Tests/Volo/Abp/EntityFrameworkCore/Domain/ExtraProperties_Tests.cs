using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Data;
using Volo.Abp.TestApp.Testing;
using Xunit;

namespace Volo.Abp.EntityFrameworkCore.Domain
{
    public class ExtraProperties_Tests : ExtraProperties_Tests<AbpEntityFrameworkCoreTestModule>
    {
        [Fact]
        public async Task Should_Get_An_Extra_Property_Configured_As_Extension()
        {
            var london = await CityRepository.FindByNameAsync("London");
            london.HasProperty("PhoneCode").ShouldBeTrue();
            london.GetProperty<string>("PhoneCode").ShouldBe("42");
        }

        [Fact]
        public async Task Should_Update_An_Existing_Extra_Property_Configured_As_Extension()
        {
            var london = await CityRepository.FindByNameAsync("London");
            london.GetProperty<string>("PhoneCode").ShouldBe("42");

            london.ExtraProperties["PhoneCode"] = "53";
            await CityRepository.UpdateAsync(london);

            var london2 = await CityRepository.FindByNameAsync("London");
            london2.GetProperty<string>("PhoneCode").ShouldBe("53");
        }
    }
}
