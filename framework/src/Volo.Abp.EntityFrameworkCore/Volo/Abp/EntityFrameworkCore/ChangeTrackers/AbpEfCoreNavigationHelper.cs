using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.EntityFrameworkCore.ChangeTrackers;

/// <summary>
/// Refactor this class after EF Core supports this case.
/// https://github.com/dotnet/efcore/issues/24076#issuecomment-1996623874
/// </summary>
public class AbpEfCoreNavigationHelper : ITransientDependency
{
    protected Dictionary<string, AbpEntityEntry> EntityEntries { get; } = new();

    public virtual void ChangeTracker_Tracked(ChangeTracker changeTracker, object? sender, EntityTrackedEventArgs e)
    {
        EntityEntryTrackedOrStateChanged(e.Entry);
        DetectChanges(changeTracker, e.Entry);
    }

    public virtual void ChangeTracker_StateChanged(ChangeTracker changeTracker, object? sender, EntityStateChangedEventArgs e)
    {
        EntityEntryTrackedOrStateChanged(e.Entry);
        DetectChanges(changeTracker, e.Entry);
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

        if (EntityEntries.ContainsKey(entryId))
        {
            return;
        }

        EntityEntries.Add(entryId, new AbpEntityEntry(entryId, entityEntry));
    }

    protected virtual void DetectChanges(ChangeTracker changeTracker, EntityEntry entityEntry)
    {
        if (entityEntry.State != EntityState.Added &&
            entityEntry.State != EntityState.Deleted &&
            entityEntry.State != EntityState.Modified)
        {
            return;
        }

#pragma warning disable EF1001
        var stateManager = changeTracker.Context.GetDependencies().StateManager;
        var internalEntityEntityEntry = stateManager.Entries.FirstOrDefault(x => x.Entity == entityEntry.Entity);
        if (internalEntityEntityEntry == null)
        {
            return;
        }

        var foreignKeys = entityEntry.Metadata.GetForeignKeys().ToList();
        foreach (var foreignKey in foreignKeys)
        {
            var principal = stateManager.FindPrincipal(internalEntityEntityEntry, foreignKey);
            if (principal == null)
            {
                continue;
            }

            var entryId = GetEntityId(principal.ToEntityEntry());
            if (entryId == null || !EntityEntries.TryGetValue(entryId, out var abpEntityEntry))
            {
                continue;
            }

            abpEntityEntry.IsModified = true;
            var navigationEntry = abpEntityEntry.NavigationEntries.FirstOrDefault(x => x.NavigationEntry.Metadata is INavigation navigationMetadata && navigationMetadata.ForeignKey == foreignKey) ??
                                  abpEntityEntry.NavigationEntries.FirstOrDefault(x => x.NavigationEntry.Metadata is ISkipNavigation skipNavigationMetadata && skipNavigationMetadata.ForeignKey == foreignKey);
            if (navigationEntry != null)
            {
                navigationEntry.IsModified = true;
            }
        }

        var skipNavigations = entityEntry.Metadata.GetSkipNavigations().ToList();
        foreach (var skipNavigation in skipNavigations)
        {
            var joinEntityType = skipNavigation.JoinEntityType;
            var foreignKey = skipNavigation.ForeignKey;
            var inverseForeignKey = skipNavigation.Inverse.ForeignKey;
            foreach (var joinEntry in stateManager.Entries)
            {
                if (joinEntry.EntityType != joinEntityType || stateManager.FindPrincipal(joinEntry, foreignKey) != internalEntityEntityEntry)
                {
                    continue;
                }

                var principal = stateManager.FindPrincipal(joinEntry, inverseForeignKey);
                if (principal == null)
                {
                    continue;
                }

                var entryId = GetEntityId(principal.ToEntityEntry());
                if (entryId == null || !EntityEntries.TryGetValue(entryId, out var abpEntityEntry))
                {
                    continue;
                }

                abpEntityEntry.IsModified = true;
                var navigationEntry = abpEntityEntry.NavigationEntries.FirstOrDefault(x => x.NavigationEntry.Metadata is INavigation navigationMetadata && navigationMetadata.ForeignKey == inverseForeignKey) ??
                                      abpEntityEntry.NavigationEntries.FirstOrDefault(x => x.NavigationEntry.Metadata is ISkipNavigation skipNavigationMetadata && skipNavigationMetadata.ForeignKey == inverseForeignKey);
                if (navigationEntry != null)
                {
                    navigationEntry.IsModified = true;
                }
            }
        }
#pragma warning restore EF1001
    }

    public virtual List<EntityEntry> GetChangedEntityEntries()
    {
        return EntityEntries
            .Where(x => x.Value.IsModified)
            .Select(x => x.Value.EntityEntry)
            .ToList();
    }

    public virtual bool IsEntityEntryModified(EntityEntry entityEntry)
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

        return EntityEntries.TryGetValue(entryId, out var abpEntityEntry) && abpEntityEntry.IsModified;
    }

    public virtual bool IsNavigationEntryModified(EntityEntry entityEntry,  int navigationEntryIndex)
    {
        var entryId = GetEntityId(entityEntry);
        if (entryId == null)
        {
            return false;
        }

        if (!EntityEntries.TryGetValue(entryId, out var abpEntityEntry))
        {
            return false;
        }

        var navigationEntryProperty = abpEntityEntry.NavigationEntries.ElementAtOrDefault(navigationEntryIndex);
        return navigationEntryProperty != null && navigationEntryProperty.IsModified;
    }

    protected virtual string? GetEntityId(EntityEntry entityEntry)
    {
        return entityEntry.Entity is IEntity entryEntity && entryEntity.GetKeys().Length == 1
            ? entryEntity.GetKeys().FirstOrDefault()?.ToString()
            : null;
    }

    public void Clear()
    {
        EntityEntries.Clear();
    }
}
