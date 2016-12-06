namespace Volo.DependencyInjection
{
    public interface IObjectAccessor<T>
    {
        T Object { get; }
    }
}