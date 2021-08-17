using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.Data
{
    public static class ConcurrencyStampExtensions
    {
        public static void SetConcurrencyStamp(this IHasConcurrencyStamp entity, string concurrencyStamp)
        {
            if (!concurrencyStamp.IsNullOrEmpty())
            {
                entity.ConcurrencyStamp = concurrencyStamp;
            }
        }
    }
}
