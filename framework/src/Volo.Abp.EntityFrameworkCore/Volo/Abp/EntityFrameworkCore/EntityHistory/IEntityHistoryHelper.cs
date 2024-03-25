using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Volo.Abp.Auditing;
using Volo.Abp.EntityFrameworkCore.ChangeTrackers;

namespace Volo.Abp.EntityFrameworkCore.EntityHistory;

public interface IEntityHistoryHelper
{
    void InitializeNavigationHelper(AbpEfCoreNavigationHelper abpEfCoreNavigationHelper);

    List<EntityChangeInfo> CreateChangeList(ICollection<EntityEntry> entityEntries);

    void UpdateChangeList(List<EntityChangeInfo> entityChanges);
}
