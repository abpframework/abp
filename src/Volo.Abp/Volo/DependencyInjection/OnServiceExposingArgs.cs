using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Volo.DependencyInjection
{
    public class OnServiceExposingArgs : IOnServiceExposingArgs
    {
        public Type ImplementationType { get; }

        public List<Type> ExposedTypes { get; }

        public OnServiceExposingArgs([NotNull] Type implementationType, List<Type> exposedTypes)
        {
            ImplementationType = Check.NotNull(implementationType, nameof(implementationType));
            ExposedTypes = Check.NotNull(exposedTypes, nameof(exposedTypes));
        }
    }
}