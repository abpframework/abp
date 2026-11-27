using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.TestBase.Logging;

namespace Volo.Abp.DynamicProxy;

public class SimpleInterceptionTargetClass : ICanLogOnObject
{
    public List<string> Logs { get; } = new List<string>();

    public virtual void DoIt()
    {
        Logs.Add("ExecutingDoIt");
    }

    public virtual int GetValue()
    {
        Logs.Add("ExecutingGetValue");
        return 42;
    }

    public virtual async Task<int> GetValueAsync()
    {
        Logs.Add("EnterGetValueAsync");
        await Task.Delay(5);
        Logs.Add("MiddleGetValueAsync");
        await Task.Delay(5);
        Logs.Add("ExitGetValueAsync");
        return 42;
    }

    public virtual async Task DoItAsync()
    {
        Logs.Add("EnterDoItAsync");
        await Task.Delay(5);
        Logs.Add("MiddleDoItAsync");
        await Task.Delay(5);
        Logs.Add("ExitDoItAsync");
    }

    public virtual async ValueTask<int> GetValueValueTaskAsync()
    {
        Logs.Add("EnterGetValueValueTaskAsync");
        await Task.Delay(5);
        Logs.Add("MiddleGetValueValueTaskAsync");
        await Task.Delay(5);
        Logs.Add("ExitGetValueValueTaskAsync");
        return 42;
    }

    public virtual async ValueTask DoItValueTaskAsync()
    {
        Logs.Add("EnterDoItValueTaskAsync");
        await Task.Delay(5);
        Logs.Add("MiddleDoItValueTaskAsync");
        await Task.Delay(5);
        Logs.Add("ExitDoItValueTaskAsync");
    }

    public virtual ValueTask<int> GetCompletedValueValueTaskAsync()
    {
        Logs.Add("ExecutingGetCompletedValueValueTaskAsync");
        return new ValueTask<int>(42);
    }

    public virtual async ValueTask ThrowValueTaskAsync()
    {
        Logs.Add("EnterThrowValueTaskAsync");
        await Task.Delay(5);
        throw new UserFriendlyException("ThrowValueTaskAsync");
    }

    public virtual ValueTask ThrowBeforeReturningValueTask()
    {
        Logs.Add("ExecutingThrowBeforeReturningValueTask");
        throw new UserFriendlyException("ThrowBeforeReturningValueTask");
    }

    public virtual ValueTask<int> GetValueWithOutParameterValueTaskAsync(out int value)
    {
        Logs.Add("ExecutingGetValueWithOutParameterValueTaskAsync");
        value = 43;
        return new ValueTask<int>(42);
    }

    public virtual async ValueTask<T> EchoValueTaskAsync<T>(T value)
    {
        Logs.Add("EnterEchoValueTaskAsync");
        await Task.Delay(5);
        Logs.Add("ExitEchoValueTaskAsync");
        return value;
    }
}
