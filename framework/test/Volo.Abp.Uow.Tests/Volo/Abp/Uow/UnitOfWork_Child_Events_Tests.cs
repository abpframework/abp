using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Testing;
using Xunit;

namespace Volo.Abp.Uow;

public class UnitOfWork_Child_Events_Tests : AbpIntegratedTest<AbpUnitOfWorkModule>
{
    private readonly IUnitOfWorkManager _unitOfWorkManager;

    public UnitOfWork_Child_Events_Tests()
    {
        _unitOfWorkManager = ServiceProvider.GetRequiredService<IUnitOfWorkManager>();
    }

    [Fact]
    public void Child_UnitOfWorks_Should_Not_Accumulate_Event_Handlers_On_The_Parent()
    {
        using (var parentUow = _unitOfWorkManager.Begin())
        {
            var failedHandlerCount = GetEventHandlerCount(parentUow, nameof(IUnitOfWork.Failed));
            var disposedHandlerCount = GetEventHandlerCount(parentUow, nameof(IUnitOfWork.Disposed));

            for (var i = 0; i < 100; i++)
            {
                using (var childUow = _unitOfWorkManager.Begin())
                {
                    childUow.Id.ShouldBe(parentUow.Id); //It's a child of the parent UOW.
                }
            }

            //Disposed child UOWs should not leave any handler behind on the parent.
            GetEventHandlerCount(parentUow, nameof(IUnitOfWork.Failed)).ShouldBe(failedHandlerCount);
            GetEventHandlerCount(parentUow, nameof(IUnitOfWork.Disposed)).ShouldBe(disposedHandlerCount);
        }
    }

    [Fact]
    public void Should_Trigger_Disposed_Event_Subscribed_Over_A_Child_UnitOfWork()
    {
        var disposed = false;

        using (var parentUow = _unitOfWorkManager.Begin())
        {
            using (var childUow = _unitOfWorkManager.Begin())
            {
                childUow.Disposed += (sender, args) => disposed = true;
            }

            disposed.ShouldBeFalse(); //The parent UOW has not been disposed yet!
        }

        disposed.ShouldBeTrue();
    }

    private static int GetEventHandlerCount(IUnitOfWork unitOfWork, string eventName)
    {
        var field = unitOfWork.GetType().GetField(eventName, BindingFlags.Instance | BindingFlags.NonPublic);
        field.ShouldNotBeNull();

        return field.GetValue(unitOfWork) is Delegate handler
            ? handler.GetInvocationList().Length
            : 0;
    }
}
