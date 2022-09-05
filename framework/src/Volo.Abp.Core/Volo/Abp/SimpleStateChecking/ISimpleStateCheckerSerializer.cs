using System.Text.Json.Nodes;

namespace Volo.Abp.SimpleStateChecking;

public interface ISimpleStateCheckerSerializer
{
    public string Serialize<TState>(ISimpleStateChecker<TState> checker)
        where TState : IHasSimpleStateCheckers<TState>;

    public ISimpleStateChecker<TState> Deserialize<TState>(JsonObject jsonObject, TState state)
        where TState : IHasSimpleStateCheckers<TState>;
}