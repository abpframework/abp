using System;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.TestApp;
using Volo.Abp.TestApp.Testing;
using Xunit;

namespace Volo.Abp.EntityFrameworkCore.Auditing;

public class Auditing_Tests : Auditing_Tests<AbpEntityFrameworkCoreTestModule>
{
    [Fact]
    public async Task Should_Not_Set_Modification_If_Properties_Generated_By_Database()
    {
        await WithUnitOfWorkAsync((async () =>
        {
            var douglas = await PersonRepository.GetAsync(TestDataBuilder.UserDouglasId);
            douglas.LastActiveTime = DateTime.Now;
        }));

        await WithUnitOfWorkAsync((async () =>
        {
            var douglas = await PersonRepository.FindAsync(TestDataBuilder.UserDouglasId);

            douglas.ShouldNotBeNull();
            douglas.LastModificationTime.ShouldBeNull();
            douglas.LastModificationTime.ShouldBeNull();
            douglas.LastModifierId.ShouldBeNull();
        }));
    }

    [Fact]
    public async Task Should_Set_Modification_If_Properties_Not_Generated_By_Database()
    {
        await WithUnitOfWorkAsync((async () =>
        {
            var douglas = await PersonRepository.GetAsync(TestDataBuilder.UserDouglasId);
            douglas.LastActiveTime = DateTime.Now;
            douglas.Age = 100;
        }));

        await WithUnitOfWorkAsync((async () =>
        {
            var douglas = await PersonRepository.FindAsync(TestDataBuilder.UserDouglasId);

            douglas.ShouldNotBeNull();
            douglas.LastModificationTime.ShouldNotBeNull();
            douglas.LastModificationTime.Value.ShouldBeLessThanOrEqualTo(Clock.Now);
            douglas.LastModifierId.ShouldBe(CurrentUserId);
        }));
    }
}
