using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Volo.Abp.EntityFrameworkCore.ChangeTrackers;

public class AbpEntityEntryNavigationProperty
{
    public int Index { get; set; }

    public string Name { get; set; }

    public object? Value { get; set; }

    public bool IsChanged { get; set; }

    public EntityEntry EntityEntry { get; set; }

    public NavigationEntry NavigationEntry { get; set; }

    public AbpEntityEntryNavigationProperty(int index, string name, object? value, bool isChanged, EntityEntry entityEntry, NavigationEntry navigationEntry)
    {
        Index = index;
        Name = name;
        Value = value;
        IsChanged = isChanged;
        EntityEntry = entityEntry;
        NavigationEntry = navigationEntry;
    }
}
