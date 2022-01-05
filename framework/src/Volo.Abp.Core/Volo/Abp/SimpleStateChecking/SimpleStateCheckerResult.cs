using System.Collections.Generic;

namespace Volo.Abp.SimpleStateChecking;

public class SimpleStateCheckerResult<TState> : Dictionary<TState, bool>
    where TState : IHasSimpleStateCheckers<TState>
{
    public SimpleStateCheckerResult()
    {

    }

    public SimpleStateCheckerResult(IEnumerable<TState> states, bool initValue = true)
    {
        foreach (var state in states)
        {
            Add(state, initValue);
        }
    }
}
