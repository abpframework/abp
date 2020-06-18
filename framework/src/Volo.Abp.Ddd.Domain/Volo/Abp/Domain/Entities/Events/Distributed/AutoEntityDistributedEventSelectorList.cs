using System.Collections.Generic;

namespace Volo.Abp.Domain.Entities.Events.Distributed
{
    public class AutoEntityDistributedEventSelectorList : List<NamedTypeSelector>, IAutoEntityDistributedEventSelectorList
    {
        public bool RemoveByName(string name)
        {
            return RemoveAll(s => s.Name == name) > 0;
        }
    }
}