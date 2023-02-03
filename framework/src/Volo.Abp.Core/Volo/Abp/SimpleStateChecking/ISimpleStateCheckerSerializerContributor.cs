using System.Text.Json.Nodes;
using JetBrains.Annotations;

namespace Volo.Abp.SimpleStateChecking;

public interface ISimpleStateCheckerSerializerContributor
{
    [CanBeNull]
    public string SerializeToJson<TState>(ISimpleStateChecker<TState> checker)
        where TState : IHasSimpleStateCheckers<TState>;

    [CanBeNull]
    public ISimpleStateChecker<TState> Deserialize<TState>(JsonObject jsonObject, TState state)
        where TState : IHasSimpleStateCheckers<TState>;
}