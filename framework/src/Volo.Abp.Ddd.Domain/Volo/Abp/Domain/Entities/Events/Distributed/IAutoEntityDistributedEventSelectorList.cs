using System;
using System.Collections.Generic;

namespace Volo.Abp.Domain.Entities.Events.Distributed;

public interface IAutoEntityDistributedEventSelectorList : IList<NamedTypeSelector>
{
}
