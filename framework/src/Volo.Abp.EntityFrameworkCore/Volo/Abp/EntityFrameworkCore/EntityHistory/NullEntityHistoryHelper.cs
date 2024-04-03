using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Volo.Abp.Auditing;
using Volo.Abp.EntityFrameworkCore.ChangeTrackers;

namespace Volo.Abp.EntityFrameworkCore.EntityHistory;

public class NullEntityHistoryHelper : IEntityHistoryHelper
{
    public static NullEntityHistoryHelper Instance { get; } = new NullEntityHistoryHelper();

    private NullEntityHistoryHelper()
    {

    }

    public void InitializeNavigationHelper(AbpEfCoreNavigationHelper abpEfCoreNavigationHelper)
    {

    }

    public List<EntityChangeInfo> CreateChangeList(ICollection<EntityEntry> entityEntries)
    {
        return new List<EntityChangeInfo>();
    }

    public void UpdateChangeList(List<EntityChangeInfo> entityChanges)
    {

    }
}
