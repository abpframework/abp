using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Testing;
using Xunit;

namespace Volo.Abp.Uow;

public class UnitOfWork_Ambient_Scope_Tests : AbpIntegratedTest<AbpUnitOfWorkModule>
{
    private readonly IUnitOfWorkManager _unitOfWorkManager;

    public UnitOfWork_Ambient_Scope_Tests()
    {
        _unitOfWorkManager = ServiceProvider.GetRequiredService<IUnitOfWorkManager>();
    }

    [Fact]
    public async Task UnitOfWorkManager_Current_Should_Set_Correctly()
    {
        _unitOfWorkManager.Current.ShouldBeNull();

        using (var uow1 = _unitOfWorkManager.Begin())
        {
            _unitOfWorkManager.Current.ShouldNotBeNull();
            _unitOfWorkManager.Current.ShouldBe(uow1);

            using (var uow2 = _unitOfWorkManager.Begin())
            {
                _unitOfWorkManager.Current.ShouldNotBeNull();
                _unitOfWorkManager.Current.Id.ShouldBe(uow1.Id);

                await uow2.CompleteAsync();
            }

            _unitOfWorkManager.Current.ShouldNotBeNull();
            _unitOfWorkManager.Current.ShouldBe(uow1);

            await uow1.CompleteAsync();
        }

        _unitOfWorkManager.Current.ShouldBeNull();
    }

    [Fact]
    public async Task UnitOfWorkManager_Reservation_Test()
    {
        _unitOfWorkManager.Current.ShouldBeNull();

        using (var uow1 = _unitOfWorkManager.Reserve("Reservation1"))
        {
            _unitOfWorkManager.Current.ShouldBeNull();

            using (var uow2 = _unitOfWorkManager.Begin())
            {
                _unitOfWorkManager.Current.ShouldNotBeNull();
                _unitOfWorkManager.Current.Id.ShouldNotBe(uow1.Id);

                await uow2.CompleteAsync();
            }

            _unitOfWorkManager.Current.ShouldBeNull();

            _unitOfWorkManager.BeginReserved("Reservation1");

            _unitOfWorkManager.Current.ShouldNotBeNull();
            _unitOfWorkManager.Current.Id.ShouldBe(uow1.Id);

            await uow1.CompleteAsync();
        }

        _unitOfWorkManager.Current.ShouldBeNull();
    }

    [Theory]
    [InlineData(true, false)]
    [InlineData(true, true)]
    [InlineData(false, true)]
    public async Task Should_Not_Begin_A_Reserved_Unit_Of_Work_That_Is_Not_Alive_Anymore(bool complete, bool dispose)
    {
        var reservationEnded = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);

        using (var uow1 = _unitOfWorkManager.Reserve("Reservation1"))
        {
            // Captures the reserved unit of work as its ambient one and keeps running after the reservation ends,
            // like the work that is started by a request and outlives it.
            var outlivingWork = Task.Run(async () =>
            {
                await reservationEnded.Task;

                var isBegun = _unitOfWorkManager.TryBeginReserved("Reservation1", new AbpUnitOfWorkOptions());

                using (var uow2 = _unitOfWorkManager.Begin())
                {
                    _unitOfWorkManager.Current.ShouldNotBeNull();
                    _unitOfWorkManager.Current.Id.ShouldNotBe(uow1.Id);

                    await uow2.CompleteAsync();
                }

                return isBegun;
            });

            if (complete)
            {
                await uow1.CompleteAsync();
            }

            if (dispose)
            {
                uow1.Dispose();
            }

            reservationEnded.SetResult();

            (await outlivingWork).ShouldBeFalse();
        }
    }
}
