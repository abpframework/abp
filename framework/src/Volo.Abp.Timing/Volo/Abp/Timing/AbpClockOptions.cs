using System;

namespace Volo.Abp.Timing
{
    public class AbpClockOptions
    {
        /// <summary>
        /// Default: <see cref="DateTimeKind.Unspecified"/>
        /// </summary>
        public DateTimeKind Kind { get; set; }

        public AbpClockOptions()
        {
            Kind = DateTimeKind.Unspecified;
        }
    }
}