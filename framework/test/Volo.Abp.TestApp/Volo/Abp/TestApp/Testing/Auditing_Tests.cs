using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Shouldly;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Modularity;
using Volo.Abp.TestApp.Domain;
using Volo.Abp.Timing;
using Volo.Abp.Users;
using Xunit;

namespace Volo.Abp.TestApp.Testing;

public abstract class Auditing_Tests<TStartupModule> : TestAppTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    protected Guid? CurrentUserId;

    protected readonly IRepository<Person, Guid> PersonRepository;
    protected readonly IClock Clock;
    protected readonly IDataFilter DataFilter;

    protected Auditing_Tests()
    {
        PersonRepository = GetRequiredService<IRepository<Person, Guid>>();
        Clock = GetRequiredService<IClock>();
        DataFilter = GetRequiredService<IDataFilter>();
    }

    protected override void AfterAddApplication(IServiceCollection services)
    {
        var currentUser = Substitute.For<ICurrentUser>();
        currentUser.Id.Returns(ci => CurrentUserId);

        services.AddSingleton(currentUser);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("4b2790fc-3f51-43d5-88a1-a92d96a9e6ea")]
    public async Task Should_Set_Creation_Properties(string currentUserId)
    {
        if (currentUserId != null)
        {
            CurrentUserId = Guid.Parse(currentUserId);
        }

        var personId = Guid.NewGuid();

        await PersonRepository.InsertAsync(new Person(personId, "Adam", 42));

        var person = await PersonRepository.FindAsync(personId);

        person.ShouldNotBeNull();
        person.CreationTime.ShouldBeLessThanOrEqualTo(Clock.Now);
        person.CreatorId.ShouldBe(CurrentUserId);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("4b2790fc-3f51-43d5-88a1-a92d96a9e6ea")]
    public async Task Should_Set_Modification_Properties(string currentUserId)
    {
        if (currentUserId != null)
        {
            CurrentUserId = Guid.Parse(currentUserId);
        }

        var douglas = await PersonRepository.GetAsync(TestDataBuilder.UserDouglasId);
        douglas.LastModificationTime.ShouldBeNull();

        douglas.Age++;

        await PersonRepository.UpdateAsync(douglas);

        douglas = await PersonRepository.FindAsync(TestDataBuilder.UserDouglasId);

        douglas.ShouldNotBeNull();
        douglas.LastModificationTime.ShouldNotBeNull();
        douglas.LastModificationTime.Value.ShouldBeLessThanOrEqualTo(Clock.Now);
        douglas.LastModifierId.ShouldBe(CurrentUserId);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("4b2790fc-3f51-43d5-88a1-a92d96a9e6ea")]
    public async Task Should_Set_Deletion_Properties(string currentUserId)
    {
        if (currentUserId != null)
        {
            CurrentUserId = Guid.Parse(currentUserId);
        }

        var douglas = await PersonRepository.GetAsync(TestDataBuilder.UserDouglasId);

        await PersonRepository.DeleteAsync(douglas);

        douglas = await PersonRepository.FindAsync(TestDataBuilder.UserDouglasId);

        douglas.ShouldBeNull();

        using (DataFilter.Disable<ISoftDelete>())
        {
            douglas = await PersonRepository.FindAsync(TestDataBuilder.UserDouglasId);

            douglas.ShouldNotBeNull();
            douglas.DeletionTime.ShouldNotBeNull();
            douglas.DeletionTime.Value.ShouldBeLessThanOrEqualTo(Clock.Now);
            douglas.DeleterId.ShouldBe(CurrentUserId);
        }
    }
}
