using JetBrains.Annotations;

namespace Volo.Abp.DependencyInjection;

public interface IObjectAccessor<out T>
{
    [CanBeNull]
    T Value { get; }
}
