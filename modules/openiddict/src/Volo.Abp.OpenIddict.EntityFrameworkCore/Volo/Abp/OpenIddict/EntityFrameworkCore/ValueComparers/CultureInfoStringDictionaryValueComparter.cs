using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Volo.Abp.OpenIddict.EntityFrameworkCore.ValueConverters
{

    public class CultureInfoStringDictionaryValueComparter : ValueComparer<Dictionary<CultureInfo, string>>
    {
        public CultureInfoStringDictionaryValueComparter()
            : base(
                  (d1, d2) => d1.SequenceEqual(d2),
                  d => d.Aggregate(0, (k, v) => HashCode.Combine(k, v.GetHashCode())),
                  d => new Dictionary<CultureInfo, string>(d))
        {
        }
    }
}
