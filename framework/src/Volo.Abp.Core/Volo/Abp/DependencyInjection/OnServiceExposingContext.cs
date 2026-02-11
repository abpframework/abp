using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Volo.Abp.DependencyInjection;

public class OnServiceExposingContext : IOnServiceExposingContext
{
    public Type ImplementationType { get; }

    public List<ServiceIdentifier> ExposedTypes { get; }

    public OnServiceExposingContext([NotNull] Type implementationType, List<Type> exposedTypes)
    {
        ImplementationType = Check.NotNull(implementationType, nameof(implementationType));
        ExposedTypes = Check.NotNull(exposedTypes, nameof(exposedTypes)).ConvertAll(t => new ServiceIdentifier(t));
    }

    public OnServiceExposingContext([NotNull] Type implementationType, List<ServiceIdentifier> exposedTypes)
    {
        ImplementationType = Check.NotNull(implementationType, nameof(implementationType));
        ExposedTypes = Check.NotNull(exposedTypes, nameof(exposedTypes));
    }
}
