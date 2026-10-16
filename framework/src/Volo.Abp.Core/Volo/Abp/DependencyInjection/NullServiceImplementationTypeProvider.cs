using System;

namespace Volo.Abp.DependencyInjection;

[Dependency(TryRegister = true)]
public class NullServiceImplementationTypeProvider : IServiceImplementationTypeProvider, ITransientDependency
{
    public Type? GetImplementationTypeOrNull(Type serviceType)
    {
        return null;
    }
}
