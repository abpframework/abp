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
    private Dictionary<string, List<AbpEntityEntryNavigationProperty>> EntityEntryNavigationProperties { get; } = new ();

    public virtual void ChangeTracker_Tracked(ChangeTracker changeTracker, object? sender, EntityTrackedEventArgs e)
    {
        foreach (var entry in changeTracker.Entries())
        {
            EntityEntryTrackedOrStateChanged(entry);
        }
    }

    public virtual void ChangeTracker_StateChanged(ChangeTracker changeTracker, object? sender, EntityStateChangedEventArgs e)
    {
        foreach (var entry in changeTracker.Entries())
        {
            EntityEntryTrackedOrStateChanged(entry);
        }
    }

    private void EntityEntryTrackedOrStateChanged(EntityEntry entityEntry)
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
            if (!navigationEntry.IsLoaded)
            {
                index++;
                continue;
            }

            var currentValue = navigationEntry.CurrentValue;
            if (navigationEntry.CurrentValue is ICollection collection)
            {
                currentValue = collection.Cast<object?>().ToList();
            }

            var navigationProperty = navigationProperties.FirstOrDefault(x => x.Index == index);
            if (navigationProperty != null)
            {
                if (!navigationProperty.IsChanged && (navigationProperty.Value == null || IsCollectionAndEmpty(navigationProperty.Value)))
                {
                    navigationProperty.Value = currentValue;
                    navigationProperty.IsChanged = currentValue != null && !IsCollectionAndEmpty(currentValue);
                }

                if (!navigationProperty.IsChanged  && navigationProperty.Value != null && !IsCollectionAndEmpty(navigationProperty.Value))
                {
                    navigationProperty.Value = currentValue;
                    navigationProperty.IsChanged = currentValue == null || IsCollectionAndEmpty(currentValue);
                }
            }
            else
            {
                navigationProperties.Add(new AbpEntityEntryNavigationProperty(index, navigationEntry.Metadata.Name, currentValue, false));
            }

            index++;
        }
    }

    public bool IsEntityEntryNavigationChanged(EntityEntry entityEntry)
    {
        if (entityEntry.State == EntityState.Modified)
        {
            return true;
        }

        var entryId = GetEntityId(entityEntry);
        if (entryId == null)
        {
            return false;
        }

        var index = 0;
        foreach (var navigationEntry in entityEntry.Navigations)
        {
            if (navigationEntry.IsModified || (navigationEntry is ReferenceEntry && navigationEntry.As<ReferenceEntry>().TargetEntry?.State == EntityState.Modified))
            {
                return true;
            }

            EntityEntryTrackedOrStateChanged(entityEntry);

            if (EntityEntryNavigationProperties.TryGetValue(entryId, out var navigationProperties))
            {
                var navigationProperty = navigationProperties.FirstOrDefault(x => x.Index == index);
                if (navigationProperty != null && navigationProperty.IsChanged)
                {
                    return true;
                }
            }

            index++;
        }

        return false;
    }

    public bool IsEntityEntryNavigationChanged(NavigationEntry navigationEntry, int index)
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
}
