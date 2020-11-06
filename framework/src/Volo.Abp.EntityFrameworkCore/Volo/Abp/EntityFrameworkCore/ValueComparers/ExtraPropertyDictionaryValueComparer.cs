using System;
using System.Linq;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Volo.Abp.Data;

namespace Volo.Abp.EntityFrameworkCore.ValueComparers
{
    public class ExtraPropertyDictionaryValueComparer : ValueComparer<ExtraPropertyDictionary>
    {
        public ExtraPropertyDictionaryValueComparer()
            : base(
                  (d1, d2) => d1.SequenceEqual(d2),
                  d => d.Aggregate(0, (k, v) => HashCode.Combine(k, v.GetHashCode())),
                  d => new ExtraPropertyDictionary(d))
        {
        }
    }
}
