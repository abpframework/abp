using System;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Volo.Abp.Timing;

namespace Volo.Abp.EntityFrameworkCore.ValueConverters;

public class AbpDateTimeValueConverter : ValueConverter<DateTime, DateTime>
{
    public AbpDateTimeValueConverter(IClock clock, ConverterMappingHints? mappingHints = null)
        : base(
            x => clock.Normalize(x),
            x => clock.Normalize(x), mappingHints)
    {
    }
}

public class AbpNullableDateTimeValueConverter : ValueConverter<DateTime?, DateTime?>
{
    public AbpNullableDateTimeValueConverter(IClock clock, ConverterMappingHints? mappingHints = null)
        : base(
            x => x.HasValue ? clock.Normalize(x.Value) : x,
            x => x.HasValue ? clock.Normalize(x.Value) : x, mappingHints)
    {
    }
}
