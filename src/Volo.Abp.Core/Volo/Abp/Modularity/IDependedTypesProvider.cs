using System;
using JetBrains.Annotations;

namespace Volo.Abp.Modularity
{
    public interface IDependedTypesProvider
    {
        [NotNull]
        Type[] GetDependedTypes();
    }
}