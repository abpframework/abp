namespace Volo.Abp.EntityFrameworkCore.ChangeTrackers;

public class AbpEntityEntryNavigationProperty
{
    public int Index { get; set; }

    public string Name { get; set; }

    public object? Value { get; set; }

    public bool IsChanged { get; set; }

    public AbpEntityEntryNavigationProperty(int index, string name, object? value, bool isChanged)
    {
        Index = index;
        Name = name;
        Value = value;
        IsChanged = isChanged;
    }
}
