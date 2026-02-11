using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
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

    public virtual void ChangeTracker_Tracked(object? sender, EntityTrackedEventArgs e)
    {
        EntityEntryTrackedOrStateChanged(e.Entry);
        DetectChanges(e.Entry);
    }

    public virtual void ChangeTracker_StateChanged(object? sender, EntityStateChangedEventArgs e)
    {
        EntityEntryTrackedOrStateChanged(e.Entry);
        DetectChanges(e.Entry);
    }

    protected virtual void EntityEntryTrackedOrStateChanged(EntityEntry entityEntry)
    {
        if (entityEntry.State != EntityState.Unchanged)
        {
            return;
        }

        var entryId = GetEntityEntryIdentity(entityEntry);
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

    protected virtual void DetectChanges(EntityEntry entityEntry, bool checkEntityEntryState = true)
    {
        #pragma warning disable EF1001
        var stateManager = entityEntry.Context.GetDependencies().StateManager;
        var internalEntityEntityEntry = stateManager.TryGetEntry(entityEntry.Entity, throwOnNonUniqueness: false);
        if (internalEntityEntityEntry == null)
        {
            return;
        }

        var foreignKeys = entityEntry.Metadata.GetForeignKeys();
        foreach (var foreignKey in foreignKeys)
        {
            var principal = stateManager.FindPrincipal(internalEntityEntityEntry, foreignKey);
            if (principal == null)
            {
                continue;
            }

            var entryId = GetEntityEntryIdentity(principal.ToEntityEntry());
            if (entryId == null || !EntityEntries.TryGetValue(entryId, out var abpEntityEntry))
            {
                continue;
            }

            var navigationEntry = abpEntityEntry.NavigationEntries.FirstOrDefault(x => x.NavigationEntry.Metadata is INavigation navigationMetadata && navigationMetadata.ForeignKey == foreignKey) ??
                                  abpEntityEntry.NavigationEntries.FirstOrDefault(x => x.NavigationEntry.Metadata is ISkipNavigation skipNavigationMetadata && skipNavigationMetadata.ForeignKey == foreignKey);

            if (navigationEntry != null && checkEntityEntryState && entityEntry.State == EntityState.Unchanged)
            {
                abpEntityEntry.UpdateNavigation(entityEntry, navigationEntry);
            }

            if (!abpEntityEntry.IsModified && (!checkEntityEntryState || IsEntityEntryChanged(entityEntry)))
            {
                abpEntityEntry.IsModified = true;
                DetectChanges(abpEntityEntry.EntityEntry, false);
            }

            if (navigationEntry != null && IsEntityEntryChanged(entityEntry))
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

                var entryId = GetEntityEntryIdentity(principal.ToEntityEntry());
                if (entryId == null || !EntityEntries.TryGetValue(entryId, out var abpEntityEntry))
                {
                    continue;
                }

                var navigationEntry = abpEntityEntry.NavigationEntries.FirstOrDefault(x => x.NavigationEntry.Metadata is INavigation navigationMetadata && navigationMetadata.ForeignKey == inverseForeignKey) ??
                                      abpEntityEntry.NavigationEntries.FirstOrDefault(x => x.NavigationEntry.Metadata is ISkipNavigation skipNavigationMetadata && skipNavigationMetadata.ForeignKey == inverseForeignKey);

                if (navigationEntry != null && checkEntityEntryState && entityEntry.State == EntityState.Unchanged)
                {
                    abpEntityEntry.UpdateNavigation(entityEntry, navigationEntry);
                }

                if (!abpEntityEntry.IsModified  && (!checkEntityEntryState || IsEntityEntryChanged(entityEntry)))
                {
                    abpEntityEntry.IsModified = true;
                    DetectChanges(abpEntityEntry.EntityEntry, false);
                }

                if (navigationEntry != null && (!checkEntityEntryState || IsEntityEntryChanged(entityEntry)))
                {
                    navigationEntry.IsModified = true;
                }
            }
        }
#pragma warning restore EF1001
    }

    protected virtual bool IsEntityEntryChanged(EntityEntry entityEntry)
    {
        return entityEntry.State == EntityState.Added ||
               entityEntry.State == EntityState.Deleted ||
               entityEntry.State == EntityState.Modified;
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

        var entryId = GetEntityEntryIdentity(entityEntry);
        if (entryId == null)
        {
            return false;
        }

        return EntityEntries.TryGetValue(entryId, out var abpEntityEntry) && abpEntityEntry.IsModified;
    }

    public virtual bool IsNavigationEntryModified(EntityEntry entityEntry, int? navigationEntryIndex = null)
    {
        var entryId = GetEntityEntryIdentity(entityEntry);
        if (entryId == null)
        {
            return false;
        }

        if (!EntityEntries.TryGetValue(entryId, out var abpEntityEntry))
        {
            return false;
        }

        if (navigationEntryIndex == null)
        {
            return abpEntityEntry.NavigationEntries.Any(x => x.IsModified);
        }

        var navigationEntryProperty = abpEntityEntry.NavigationEntries.ElementAtOrDefault(navigationEntryIndex.Value);
        return navigationEntryProperty != null && navigationEntryProperty.IsModified;
    }

    public virtual AbpNavigationEntry? GetNavigationEntry(EntityEntry entityEntry, int navigationEntryIndex)
    {
        var entryId = GetEntityEntryIdentity(entityEntry);
        if (entryId == null)
        {
            return null;
        }

        if (!EntityEntries.TryGetValue(entryId, out var abpEntityEntry))
        {
            return null;
        }

        return abpEntityEntry.NavigationEntries.ElementAtOrDefault(navigationEntryIndex);
    }

    protected virtual string? GetEntityEntryIdentity(EntityEntry entityEntry)
    {
        if (entityEntry.Entity is IEntity entryEntity && entryEntity.GetKeys().Length == 1)
        {
            return $"{entityEntry.Metadata.ClrType.FullName}:{entryEntity.GetKeys().FirstOrDefault()}";
        }

        return null;
    }

    public virtual void RemoveChangedEntityEntries()
    {
        EntityEntries.RemoveAll(x => x.Value.IsModified);
    }

    public virtual void Clear()
    {
        EntityEntries.Clear();
    }
}
