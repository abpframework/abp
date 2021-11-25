using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Volo.Abp.Timing;

namespace Volo.Abp.EntityFrameworkCore.ValueConverters;

public class AbpDateTimeValueConverter : ValueConverter<DateTime?, DateTime?>
{
    public AbpDateTimeValueConverter(IClock clock, [CanBeNull] ConverterMappingHints mappingHints = null)
        : base(
            x => x.HasValue ? clock.Normalize(x.Value) : x,
            x => x.HasValue ? clock.Normalize(x.Value) : x, mappingHints)
    {
    }
}
