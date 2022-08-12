namespace Volo.Abp.SimpleStateChecking;

public interface ISimpleStateCheckerSerializer
{
    public string Serialize<TState>(ISimpleStateChecker<TState> checker)
        where TState : IHasSimpleStateCheckers<TState>;

    public ISimpleStateChecker<TState> Deserialize<TState>(string value)
        where TState : IHasSimpleStateCheckers<TState>;
}