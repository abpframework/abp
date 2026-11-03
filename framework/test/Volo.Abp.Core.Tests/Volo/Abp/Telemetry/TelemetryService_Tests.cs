using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Shouldly;
using Volo.Abp.Internal.Telemetry;
using Volo.Abp.Internal.Telemetry.Activity;
using Volo.Abp.Internal.Telemetry.Activity.Contracts;
using Xunit;

namespace Volo.Abp.Telemetry;

public class TelemetryService_Tests
{
    [Theory]
    [InlineData(TaskStatus.RanToCompletion)]
    [InlineData(TaskStatus.Faulted)]
    [InlineData(TaskStatus.Canceled)]
    public void Should_Unsubscribe_And_Release_Service_When_Activity_Completes(TaskStatus status)
    {
        using var started = new ManualResetEventSlim();
        using var release = new ManualResetEventSlim();
        var serviceProvider = Substitute.For<IServiceProvider>();
        var builder = Substitute.For<ITelemetryActivityEventBuilder>();
        builder.BuildAsync(Arg.Any<ActivityContext>()).Returns(Task.FromResult<ActivityEvent?>(null));
        serviceProvider.GetService(typeof(ITelemetryActivityEventBuilder)).Returns(builder);
        serviceProvider.GetService(typeof(ITelemetryActivityStorage)).Returns(Substitute.For<ITelemetryActivityStorage>());
        serviceProvider.GetService(typeof(ITelemetryActivitySender)).Returns(Substitute.For<ITelemetryActivitySender>());
        var scope = Substitute.For<IServiceScope>();
        scope.ServiceProvider.Returns(serviceProvider);
        var scopeFactory = Substitute.For<IServiceScopeFactory>();
        scopeFactory.CreateScope().Returns(_ =>
        {
            started.Set();
            if (!release.Wait(TimeSpan.FromSeconds(10)))
            {
                throw new TimeoutException();
            }

            if (status == TaskStatus.Faulted)
            {
                throw new InvalidOperationException();
            }

            if (status == TaskStatus.Canceled)
            {
                throw new OperationCanceledException();
            }

            return scope;
        });

        var service = StartActivity(scopeFactory);
        try
        {
            started.Wait(TimeSpan.FromSeconds(10)).ShouldBeTrue();
            GetProcessExitHandlers().Length.ShouldBe(1);
            GetCancelKeyPressHandlers().Length.ShouldBe(1);
        }
        finally
        {
            release.Set();
        }

        WaitForHandlersToBeRemoved();
        for (var i = 0; i < 3; i++)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        service.IsAlive.ShouldBeFalse();
    }

    [Theory]
    [InlineData(TaskStatus.RanToCompletion)]
    [InlineData(TaskStatus.Faulted)]
    [InlineData(TaskStatus.Canceled)]
    public void Should_Not_Leave_Handlers_For_Already_Completed_Tasks(TaskStatus status)
    {
        var task = status switch
        {
            TaskStatus.Faulted => Task.FromException(new InvalidOperationException()),
            TaskStatus.Canceled => Task.FromCanceled(new CancellationToken(true)),
            _ => Task.CompletedTask
        };

        RegisterExitHandlers(task);

        GetProcessExitHandlers().ShouldBeEmpty();
        GetCancelKeyPressHandlers().ShouldBeEmpty();
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task Should_Wait_For_Pending_Task_On_Exit(bool cancelKeyPress)
    {
        var completion = new TaskCompletionSource<bool>();
        var handlerStarted = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
        RegisterExitHandlers(completion.Task);
        var handler = cancelKeyPress ? GetCancelKeyPressHandlers().Single() : GetProcessExitHandlers().Single();
        var handlerTask = Task.Run(() =>
        {
            handlerStarted.SetResult(true);
            if (handler is ConsoleCancelEventHandler consoleHandler)
            {
                consoleHandler(null, null!);
            }
            else
            {
                ((EventHandler)handler)(null, EventArgs.Empty);
            }
        });

        try
        {
            await handlerStarted.Task.WaitAsync(TimeSpan.FromSeconds(10));
            await Task.Delay(100);
            handlerTask.IsCompleted.ShouldBeFalse();
        }
        finally
        {
            completion.TrySetResult(true);
        }

        await handlerTask.WaitAsync(TimeSpan.FromSeconds(10));
        WaitForHandlersToBeRemoved();
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static WeakReference StartActivity(IServiceScopeFactory scopeFactory)
    {
        var service = new TelemetryService(scopeFactory);
        service.AddActivityAsync("TestActivity").GetAwaiter().GetResult();
        return new WeakReference(service);
    }

    private static void RegisterExitHandlers(Task task)
    {
        typeof(TelemetryService).GetMethod("WaitForTelemetryTaskOnExit", BindingFlags.NonPublic | BindingFlags.Static)!
            .Invoke(null, new object[] { task });
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static void WaitForHandlersToBeRemoved()
    {
        SpinWait.SpinUntil(() => GetProcessExitHandlers().Length == 0 && GetCancelKeyPressHandlers().Length == 0,
            TimeSpan.FromSeconds(10)).ShouldBeTrue();
    }

    private static Delegate[] GetProcessExitHandlers()
    {
        return typeof(AppDomain).GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static)
            .Where(field => typeof(Delegate).IsAssignableFrom(field.FieldType))
            .Select(field => field.GetValue(field.IsStatic ? null : AppDomain.CurrentDomain))
            .OfType<Delegate>()
            .SelectMany(handler => handler.GetInvocationList())
            .Where(IsTelemetryHandler)
            .ToArray();
    }

    private static Delegate[] GetCancelKeyPressHandlers()
    {
        return typeof(Console).GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static)
            .Where(field => typeof(Delegate).IsAssignableFrom(field.FieldType))
            .Select(field => field.GetValue(null))
            .OfType<Delegate>()
            .SelectMany(handler => handler.GetInvocationList())
            .Where(IsTelemetryHandler)
            .ToArray();
    }

    private static bool IsTelemetryHandler(Delegate handler)
    {
        return handler.Target?.GetType().DeclaringType == typeof(TelemetryService);
    }
}
