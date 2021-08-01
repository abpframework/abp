using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Volo.Abp.OpenIddict.EntityFrameworkCore.ValueConverters
{
    public class HashSetStringValueComparter : ValueComparer<HashSet<string>>
    {
        public HashSetStringValueComparter()
            : base(
                  (d1, d2) => d1.SequenceEqual(d2),
                  d => d.Aggregate(0, (k, v) => HashCode.Combine(k, v.GetHashCode())),
                  d => new HashSet<string>(d))
        {
        }
    }
}
