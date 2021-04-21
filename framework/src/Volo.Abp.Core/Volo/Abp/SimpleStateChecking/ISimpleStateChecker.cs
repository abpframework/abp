namespace Volo.Abp.SimpleStateChecking
{
    public interface ISimpleStateChecker<TState>
        where TState : IHasSimpleStateCheckers<TState>
    {

    }
}
