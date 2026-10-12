using System;

namespace Volo.Abp.DependencyInjection;

public interface IServiceImplementationTypeProvider
{
    /// <summary>
    /// Gets the type of every instance the container creates for <paramref name="serviceType"/>, without creating one.
    /// Returns null when the registration does not fix the type (such as a factory method or a decorator) or the
    /// container can not tell it.
    /// </summary>
    Type? GetImplementationTypeOrNull(Type serviceType);
}
