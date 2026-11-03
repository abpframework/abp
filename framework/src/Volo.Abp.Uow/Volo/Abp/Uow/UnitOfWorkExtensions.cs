using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Volo.Abp.Uow;

public static class UnitOfWorkExtensions
{
    private const string ActiveChildUnitOfWorkCountItemKey = "_AbpActiveChildUnitOfWorkCount";

    public static bool IsReservedFor([NotNull] this IUnitOfWork unitOfWork, string reservationName)
    {
        Check.NotNull(unitOfWork, nameof(unitOfWork));

        return unitOfWork.IsReserved && unitOfWork.ReservationName == reservationName;
    }

    /// <summary>
    /// Checks if there is an active (not yet disposed) child unit of work scope over the given
    /// unit of work, i.e. a scope created by <see cref="IUnitOfWorkManager.Begin"/> without
    /// requiresNew while this unit of work was current. Such a scope shares this unit of work,
    /// so it should not be completed while the scope is still active.
    /// </summary>
    public static bool HasActiveChildUnitOfWorks([NotNull] this IUnitOfWork unitOfWork)
    {
        Check.NotNull(unitOfWork, nameof(unitOfWork));

        return unitOfWork.Items.GetOrDefault(ActiveChildUnitOfWorkCountItemKey) is int count && count > 0;
    }

    internal static void IncrementActiveChildUnitOfWorkCount(this IUnitOfWork unitOfWork)
    {
        var count = unitOfWork.Items.GetOrDefault(ActiveChildUnitOfWorkCountItemKey) as int? ?? 0;
        unitOfWork.Items[ActiveChildUnitOfWorkCountItemKey] = count + 1;
    }

    internal static void DecrementActiveChildUnitOfWorkCount(this IUnitOfWork unitOfWork)
    {
        var count = unitOfWork.Items.GetOrDefault(ActiveChildUnitOfWorkCountItemKey) as int? ?? 0;
        unitOfWork.Items[ActiveChildUnitOfWorkCountItemKey] = Math.Max(0, count - 1);
    }

    public static void AddItem<TValue>([NotNull] this IUnitOfWork unitOfWork, string key, TValue value)
        where TValue : class
    {
        Check.NotNull(unitOfWork, nameof(unitOfWork));

        unitOfWork.Items[key] = value;
    }

    public static TValue GetItemOrDefault<TValue>([NotNull] this IUnitOfWork unitOfWork, string key)
        where TValue : class
    {
        Check.NotNull(unitOfWork, nameof(unitOfWork));

        return unitOfWork.Items.FirstOrDefault(x => x.Key == key).Value.As<TValue>();
    }

    public static TValue GetOrAddItem<TValue>([NotNull] this IUnitOfWork unitOfWork, string key, Func<string, TValue> factory)
        where TValue : class
    {
        Check.NotNull(unitOfWork, nameof(unitOfWork));

        return unitOfWork.Items.GetOrAdd(key, factory).As<TValue>();
    }

    public static void RemoveItem([NotNull] this IUnitOfWork unitOfWork, string key)
    {
        Check.NotNull(unitOfWork, nameof(unitOfWork));

        unitOfWork.Items.RemoveAll(x => x.Key == key);
    }
}
