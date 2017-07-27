using System;
using System.Collections.Generic;

namespace Volo.DependencyInjection
{
    public interface IOnServiceExposingArgs
    {
        Type ImplementationType { get; }

        List<Type> ExposedTypes { get; }
    }
}