using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Volo.Abp.Auditing;

namespace Volo.Abp.EntityFrameworkCore.EntityHistory;

public interface IEntityHistoryHelper
{
    List<EntityChangeInfo> CreateChangeList(ICollection<EntityEntry> entityEntries);

    void UpdateChangeList(List<EntityChangeInfo> entityChanges);
}
