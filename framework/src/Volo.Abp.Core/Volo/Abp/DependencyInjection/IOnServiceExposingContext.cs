using System;
using System.Collections.Generic;

namespace Volo.Abp.DependencyInjection;

public interface IOnServiceExposingContext
{
    Type ImplementationType { get; }

    List<Type> ExposedTypes { get; }
}
