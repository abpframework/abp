using System.Collections.Generic;

namespace Volo.Abp.Auditing;

public interface IEntityHistorySelectorList : IList<NamedTypeSelector>
{
    /// <summary>
    /// Removes a selector by name.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    bool RemoveByName(string name);
}
