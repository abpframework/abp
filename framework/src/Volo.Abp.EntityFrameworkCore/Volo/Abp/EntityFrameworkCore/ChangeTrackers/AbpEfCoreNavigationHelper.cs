using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.EntityFrameworkCore.ChangeTrackers;

/// <summary>
/// Refactor this class after EF Core supports this case.
/// https://github.com/dotnet/efcore/issues/24076#issuecomment-1996623874
/// </summary>
public class AbpEfCoreNavigationHelper : ITransientDependency
{
    protected Dictionary<string, List<AbpEntityEntryNavigationProperty>> EntityEntryNavigationProperties { get; } = new Dictionary<string, List<AbpEntityEntryNavigationProperty>>();

    public virtual void ChangeTracker_Tracked(ChangeTracker changeTracker, object? sender, EntityTrackedEventArgs e)
    {
        EntityEntryTrackedOrStateChanged(e.Entry);
        DetectChanges();
    }

    public virtual void ChangeTracker_StateChanged(ChangeTracker changeTracker, object? sender, EntityStateChangedEventArgs e)
    {
        EntityEntryTrackedOrStateChanged(e.Entry);
        DetectChanges();
    }

    protected virtual void EntityEntryTrackedOrStateChanged(EntityEntry entityEntry)
    {
        if (entityEntry.State != EntityState.Unchanged)
        {
            return;
        }

        var entryId = GetEntityId(entityEntry);
        if (entryId == null)
        {
            return;
        }

        var navigationProperties = EntityEntryNavigationProperties.GetOrAdd(entryId, () => new List<AbpEntityEntryNavigationProperty>());
        var index = 0;
        foreach (var navigationEntry in entityEntry.Navigations.Where(navigation => !navigation.IsModified))
        {
            var navigationProperty = navigationProperties.FirstOrDefault(x => x.Index == index);
            if (navigationProperty != null)
            {
                if (navigationProperty.Value == null || IsCollectionAndEmpty(navigationProperty.Value))
                {
                    navigationProperty.Value = navigationEntry.CurrentValue;
                }
            }
            else
            {
                navigationProperties.Add(new AbpEntityEntryNavigationProperty(index, navigationEntry.Metadata.Name, navigationEntry.CurrentValue, false, entityEntry, navigationEntry));
            }

            index++;
        }
    }

    protected virtual void DetectChanges()
    {
        foreach (var entityEntryNavigationProperties in EntityEntryNavigationProperties)
        {
            foreach (var navigationProperty in entityEntryNavigationProperties.Value.Where(x => !x.IsChanged && x.EntityEntry.State == EntityState.Unchanged))
            {
                if (navigationProperty.NavigationEntry.IsModified)
                {
                    navigationProperty.IsChanged = true;
                    continue;
                }

                if (navigationProperty.Value == null || IsCollectionAndEmpty(navigationProperty.Value))
                {
                    if (navigationProperty.NavigationEntry.CurrentValue != null || IsCollectionAndNotEmpty(navigationProperty.NavigationEntry.CurrentValue))
                    {
                        if (navigationProperty.NavigationEntry.CurrentValue is ICollection collection)
                        {
                            navigationProperty.Value = collection.Cast<object?>().ToList();
                        }
                        else
                        {
                            navigationProperty.Value = navigationProperty.NavigationEntry.CurrentValue;
                        }
                    }
                }

                if (navigationProperty.Value != null || IsCollectionAndNotEmpty(navigationProperty.Value))
                {
                    if (navigationProperty.NavigationEntry.CurrentValue == null || IsCollectionAndEmpty(navigationProperty.NavigationEntry.CurrentValue))
                    {
                        if (IsCollectionAndEmpty(navigationProperty.Value) && IsCollectionAndEmpty(navigationProperty.NavigationEntry.CurrentValue))
                        {
                            continue;
                        }

                        navigationProperty.IsChanged = true;
                    }
                }
            }
        }
    }

    public virtual List<EntityEntry> GetChangedEntityEntries()
    {
        DetectChanges();
        return EntityEntryNavigationProperties
            .SelectMany(x => x.Value)
            .Where(x => x.NavigationEntry.IsModified || x.IsChanged)
            .Select(x => x.EntityEntry)
            .ToList();
    }

    public virtual bool IsEntityEntryNavigationChanged(EntityEntry entityEntry)
    {
        DetectChanges();
        if (entityEntry.State == EntityState.Modified)
        {
            return true;
        }

        var entryId = GetEntityId(entityEntry);
        if (entryId == null)
        {
            return false;
        }

        if (EntityEntryNavigationProperties.TryGetValue(entryId, out var navigationProperties))
        {
            return navigationProperties.Any(x => x.IsChanged) ||
                   navigationProperties.Any(x => x.NavigationEntry.IsModified) ||
                   navigationProperties.Any(x =>
                       x.NavigationEntry is ReferenceEntry &&
                       x.NavigationEntry.As<ReferenceEntry>().TargetEntry?.State == EntityState.Modified);
        }

        return false;
    }

    public virtual bool IsEntityEntryNavigationChanged(NavigationEntry navigationEntry, int index)
    {
        if (navigationEntry.IsModified || (navigationEntry is ReferenceEntry && navigationEntry.As<ReferenceEntry>().TargetEntry?.State == EntityState.Modified))
        {
            return true;
        }

        var entryId = GetEntityId(navigationEntry.EntityEntry);
        if (entryId == null)
        {
            return false;
        }

        if (EntityEntryNavigationProperties.TryGetValue(entryId, out var navigationProperties))
        {
            var navigationProperty = navigationProperties.FirstOrDefault(x => x.Index == index);
            if (navigationProperty != null && navigationProperty.IsChanged)
            {
                return true;
            }
        }

        return false;
    }

    public void Clear()
    {
        EntityEntryNavigationProperties.Clear();
    }

    private string? GetEntityId(EntityEntry entityEntry)
    {
        return entityEntry.Entity is IEntity entryEntity && entryEntity.GetKeys().Length == 1
            ? entryEntity.GetKeys().FirstOrDefault()?.ToString()
            : null;
    }

    private bool IsCollectionAndEmpty(object? value)
    {
        return value is ICollection && value is ICollection collection && collection.Count == 0;
    }

    private bool IsCollectionAndNotEmpty(object? value)
    {
        return value is ICollection && value is ICollection collection && collection.Count != 0;
    }
}
