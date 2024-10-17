using System;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.Data;

public static class ConcurrencyStampExtensions
{
    public static void SetConcurrencyStampIfNotNull(this IHasConcurrencyStamp entity, string? concurrencyStamp)
    {
        if (!concurrencyStamp.IsNullOrEmpty())
        {
            entity.ConcurrencyStamp = concurrencyStamp!;
        }
    }
}
