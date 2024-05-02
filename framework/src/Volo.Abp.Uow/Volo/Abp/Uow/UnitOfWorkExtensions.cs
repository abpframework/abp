using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Volo.Abp.Uow;

public static class UnitOfWorkExtensions
{
    public static bool IsReservedFor([NotNull] this IUnitOfWork unitOfWork, string reservationName)
    {
        Check.NotNull(unitOfWork, nameof(unitOfWork));

        return unitOfWork.IsReserved && unitOfWork.ReservationName == reservationName;
    }

    public static void AddItem<TValue>([NotNull] this IUnitOfWork unitOfWork, string key, TValue value)
        where TValue : class
    {
        Check.NotNull(unitOfWork, nameof(unitOfWork));

        if (!unitOfWork.Items.ContainsKey(key))
        {
            unitOfWork.Items[key] = value;
        }
        else
        {
            unitOfWork.Items.Add(key, value);
        }
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
