using System;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.TestApp;
using Volo.Abp.TestApp.Domain;
using Volo.Abp.Uow;
using Xunit;

namespace Volo.Abp.EntityFrameworkCore.Uow;

public interface IValueTaskPeopleAppService : IApplicationService
{
    ValueTask<Guid?> GetUnitOfWorkIdAfterAwaitAsync();

    ValueTask IncreaseAgeAsync(Guid id);

    ValueTask IncreaseAgeThenFailAsync(Guid id);
}

public class ValueTaskPeopleAppService : ApplicationService, IValueTaskPeopleAppService
{
    private readonly IRepository<Person, Guid> _personRepository;

    public ValueTaskPeopleAppService(IRepository<Person, Guid> personRepository)
    {
        _personRepository = personRepository;
    }

    public virtual async ValueTask<Guid?> GetUnitOfWorkIdAfterAwaitAsync()
    {
        await Task.Delay(5);
        return CurrentUnitOfWork?.Id;
    }

    public virtual async ValueTask IncreaseAgeAsync(Guid id)
    {
        await Task.Delay(5);
        var person = await _personRepository.GetAsync(id);
        person.Age += 100;
    }

    public virtual async ValueTask IncreaseAgeThenFailAsync(Guid id)
    {
        await Task.Delay(5);
        var person = await _personRepository.GetAsync(id);
        person.Age += 100;
        await _personRepository.UpdateAsync(person);
        throw new InvalidOperationException("Failed after the update");
    }
}

public class ValueTask_UnitOfWork_Interception_Tests : EntityFrameworkCoreTestBase
{
    private readonly IValueTaskPeopleAppService _appService;
    private readonly IRepository<Person, Guid> _personRepository;
    private readonly IUnitOfWorkManager _unitOfWorkManager;

    public ValueTask_UnitOfWork_Interception_Tests()
    {
        _appService = GetRequiredService<IValueTaskPeopleAppService>();
        _personRepository = GetRequiredService<IRepository<Person, Guid>>();
        _unitOfWorkManager = GetRequiredService<IUnitOfWorkManager>();
    }

    [Fact]
    public async Task Should_Keep_Unit_Of_Work_Until_ValueTask_Completes()
    {
        _unitOfWorkManager.Current.ShouldBeNull();

        var unitOfWorkId = await _appService.GetUnitOfWorkIdAfterAwaitAsync();

        unitOfWorkId.ShouldNotBeNull();
        _unitOfWorkManager.Current.ShouldBeNull();
    }

    [Fact]
    public async Task Should_Save_Changes_When_ValueTask_Completes()
    {
        await _appService.IncreaseAgeAsync(TestDataBuilder.UserDouglasId);

        var douglas = await _personRepository.GetAsync(TestDataBuilder.UserDouglasId);
        douglas.Age.ShouldBe(142);
    }

    [Fact]
    public async Task Should_Not_Save_Changes_When_ValueTask_Fails()
    {
        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            await _appService.IncreaseAgeThenFailAsync(TestDataBuilder.UserDouglasId);
        });

        var douglas = await _personRepository.GetAsync(TestDataBuilder.UserDouglasId);
        douglas.Age.ShouldBe(42);
    }
}
