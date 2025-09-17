using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NSubstitute;
using Shouldly;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.TestApp;
using Volo.Abp.TestApp.Testing;
using Xunit;

namespace Volo.Abp.EntityFrameworkCore.Auditing;

public class Auditing_Tests : Auditing_Tests<AbpEntityFrameworkCoreTestModule>
{
    protected IEntityChangeEventHelper EntityChangeEventHelper;

    protected override void AfterAddApplication(IServiceCollection services)
    {
        EntityChangeEventHelper = Substitute.For<IEntityChangeEventHelper>();
        services.Replace(ServiceDescriptor.Singleton(EntityChangeEventHelper));

        base.AfterAddApplication(services);
    }

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
            douglas.LastModifierId.ShouldBeNull();
        }));
    }

    [Fact]
    public async Task Should_Set_Modification_If_Properties_Changed_With_Default_Value()
    {
        var date = DateTime.Parse("2022-01-01");
        await WithUnitOfWorkAsync((async () =>
        {
            var douglas = await PersonRepository.GetAsync(TestDataBuilder.UserDouglasId);
            douglas.HasDefaultValue = date;
        }));

        await WithUnitOfWorkAsync((async () =>
        {
            var douglas = await PersonRepository.FindAsync(TestDataBuilder.UserDouglasId);

            douglas.ShouldNotBeNull();
            douglas.HasDefaultValue.ShouldBe(date);
            douglas.LastModificationTime.ShouldNotBeNull();
            douglas.LastModificationTime.Value.ShouldBeLessThanOrEqualTo(Clock.Now);
            douglas.LastModifierId.ShouldBe(CurrentUserId);
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

    [Fact]
    public async Task Should_Not_Set_Modification_If_Properties_HasDisableAuditing_UpdateModificationProps()
    {
        await WithUnitOfWorkAsync((async () =>
        {
            var douglas = await PersonRepository.GetAsync(TestDataBuilder.UserDouglasId);
            douglas.DisableAuditingUpdateModificationPropsProperty = Guid.NewGuid().ToString();
        }));

        await WithUnitOfWorkAsync((async () =>
        {
            var douglas = await PersonRepository.FindAsync(TestDataBuilder.UserDouglasId);

            douglas.ShouldNotBeNull();
            douglas.LastModificationTime.ShouldBeNull();
            douglas.LastModifierId.ShouldBeNull();
        }));

        EntityChangeEventHelper.Received().PublishEntityUpdatedEvent(Arg.Any<object>());
    }

    [Fact]
    public async Task Should_Not_PublishEntityEvent_If_Properties_HasDisableAuditing_PublishEntityEventProperty()
    {
        await WithUnitOfWorkAsync((async () =>
        {
            var douglas = await PersonRepository.GetAsync(TestDataBuilder.UserDouglasId);
            douglas.DisableAuditingPublishEntityEventProperty = Guid.NewGuid().ToString();
        }));

        await WithUnitOfWorkAsync((async () =>
        {
            var douglas = await PersonRepository.FindAsync(TestDataBuilder.UserDouglasId);

            douglas.ShouldNotBeNull();
            douglas.LastModificationTime.ShouldNotBeNull();
        }));

        EntityChangeEventHelper.DidNotReceive().PublishEntityUpdatedEvent(Arg.Any<object>());
    }


    [Fact]
    public async Task Should_Set_Modification_And_PublishEntityEvent_If_Properties_HasDisableAuditing()
    {
        await WithUnitOfWorkAsync((async () =>
        {
            var douglas = await PersonRepository.GetAsync(TestDataBuilder.UserDouglasId);
            douglas.DisableAuditingProperty = Guid.NewGuid().ToString();
        }));

        await WithUnitOfWorkAsync((async () =>
        {
            var douglas = await PersonRepository.FindAsync(TestDataBuilder.UserDouglasId);

            douglas.ShouldNotBeNull();
            douglas.LastModificationTime.ShouldNotBeNull();
        }));

        EntityChangeEventHelper.Received().PublishEntityUpdatedEvent(Arg.Any<object>());
    }
}
