using System;
using JetBrains.Annotations;

namespace Volo.Abp.Localization;

public interface IInheritedResourceTypesProvider
{
    [NotNull]
    Type[] GetInheritedResourceTypes();
}
