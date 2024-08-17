namespace Volo.Abp.DependencyInjection;

public interface IObjectAccessor<out T>
{
    T? Value { get; }
}
