using System;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Volo.Abp.Timing;

namespace Volo.Abp.EntityFrameworkCore.ValueConverters;

public class AbpDateTimeValueConverter : ValueConverter<DateTime, DateTime>
{
    public static IClock? Clock { get; set; }

    public AbpDateTimeValueConverter(ConverterMappingHints? mappingHints = null)
        : base(
            x => Clock!.Normalize(x),
            x => Clock!.Normalize(x),
            mappingHints)
    {
    }
}

public class AbpNullableDateTimeValueConverter : ValueConverter<DateTime?, DateTime?>
{
    public static IClock? Clock { get; set; }

    public AbpNullableDateTimeValueConverter(ConverterMappingHints? mappingHints = null)
        : base(
            x => x.HasValue ? Clock!.Normalize(x.Value) : x,
            x => x.HasValue ? Clock!.Normalize(x.Value) : x,
            mappingHints)
    {
    }
}
