using System;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.TestApp.Domain;
using Volo.Abp.TestApp.Testing;
using Xunit;

namespace Volo.Abp.EntityFrameworkCore.Domain;

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

        london.ExtraProperties["PhoneCode"] = 123456;
        london.ExtraProperties["Rank"] = "88";
        london.ExtraProperties["ZipCode"] = null;
        london.ExtraProperties["Established"] = DateTime.MinValue;
        london.ExtraProperties["Guid"] = "a7ae2efe-d8d6-466b-92e3-da14aa6e1c5b";
        london.ExtraProperties["EnumNumber"] = 2L;
        london.ExtraProperties["EnumNumberString"] = "2";
        london.ExtraProperties["EnumLiteral"] = "White";
        await CityRepository.UpdateAsync(london);

        var london2 = await CityRepository.FindByNameAsync("London");
        london2.GetProperty<string>("PhoneCode").ShouldBe("123456");
        london2.GetProperty<int>("Rank").ShouldBe(88);
        london2.GetProperty<string>("ZipCode").ShouldBe(null);
        london2.GetProperty<DateTime?>("Established").ShouldBe(DateTime.MinValue);
        london2.GetProperty<Guid>("Guid").ShouldBe(new Guid("a7ae2efe-d8d6-466b-92e3-da14aa6e1c5b"));
        london2.GetProperty<Color>("EnumNumber").ShouldBe(Color.White);
        london2.GetProperty<Color>("EnumNumberString").ShouldBe(Color.White);
        london2.GetProperty<Color>("EnumLiteral").ShouldBe(Color.White);
    }

    [Fact]
    public async Task An_Extra_Property_Configured_As_Extension2()
    {
        await WithUnitOfWorkAsync(async () =>
        {
            var entityEntry = (await CityRepository.GetDbContextAsync()).Attach(new City(Guid.NewGuid(), "NewYork"));
            var indexes = entityEntry.Metadata.GetIndexes().ToList();
            indexes.ShouldNotBeEmpty();
            indexes.ShouldContain(x => x.IsUnique);
        });
    }

    public enum Color
    {
        Red = 0,
        Blue = 1,
        White = 2
    }
}
