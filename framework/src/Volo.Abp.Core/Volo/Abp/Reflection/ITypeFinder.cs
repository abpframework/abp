using System;
using System.Collections.Generic;

namespace Volo.Abp.Reflection;

/// <summary>
/// Used to get types in the application.
/// It may not return all types, but those are related with modules.
/// </summary>
public interface ITypeFinder
{
    IReadOnlyList<Type> Types { get; }
}
