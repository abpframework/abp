using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Volo.Abp.EntityFrameworkCore.ChangeTrackers;

public class AbpEntityEntry
{
    public string Id { get; set; }

    public EntityEntry EntityEntry { get; set; }

    public List<AbpNavigationEntry> NavigationEntries { get; set; }

    private bool _isModified;
    public bool IsModified
    {
        get
        {
            return _isModified || EntityEntry.State == EntityState.Modified || NavigationEntries.Any(n => n.IsModified);
        }
        set => _isModified = value;
    }

    public AbpEntityEntry(string id, EntityEntry entityEntry)
    {
        Id = id;
        EntityEntry = entityEntry;
        NavigationEntries = EntityEntry.Navigations.Select(x => new AbpNavigationEntry(x, x.Metadata.Name)).ToList();
    }

    public void UpdateNavigation(EntityEntry entityEntry, AbpNavigationEntry navigationEntry)
    {
        if (IsModified ||
            EntityEntry.State == EntityState.Modified ||
            navigationEntry.IsModified)
        {
            return;
        }

        var currentValue = navigationEntry.NavigationEntry.CurrentValue;
        if (currentValue == null)
        {
            return;
        }

        if (navigationEntry.NavigationEntry is CollectionEntry)
        {
            navigationEntry.OriginalValue ??= new List<object>();
            navigationEntry.OriginalValue.As<List<object>>().Add(entityEntry.Entity);
        }
        else
        {
            navigationEntry.OriginalValue = currentValue;
        }
    }
}

public class AbpNavigationEntry
{
    public NavigationEntry NavigationEntry { get; set; }

    public string Name { get; set; }

    public bool IsModified { get; set; }

    public object? OriginalValue { get; set; }

    public object? CurrentValue => NavigationEntry.CurrentValue;

    public AbpNavigationEntry(NavigationEntry navigationEntry, string name)
    {
        NavigationEntry = navigationEntry;
        Name = name;
        if (navigationEntry.CurrentValue != null )
        {
            OriginalValue = navigationEntry is CollectionEntry ? new List<object>() : navigationEntry.CurrentValue;
        }
    }
}
