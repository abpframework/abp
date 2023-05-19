using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities;
using JetBrains.Annotations;

namespace Volo.Abp.Data;

public static class ConcurrencyStampExtensions
{
    public static void SetConcurrencyStampIfNotNull(this IHasConcurrencyStamp entity, [CanBeNull] string concurrencyStamp)
    {
        if (!concurrencyStamp.IsNullOrEmpty())
        {
            entity.ConcurrencyStamp = concurrencyStamp;
        }
    }
}
