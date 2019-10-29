namespace System
{
    /// <summary>
    /// Extension methods for the <see cref="DateTime"/>.
    /// </summary>
    public static class AbpDateTimeExtensions
    {
        public static DateTime ClearTime(this DateTime dateTime)
        {
            return dateTime.Subtract(
                new TimeSpan(
                    0,
                    dateTime.Hour,
                    dateTime.Minute,
                    dateTime.Second,
                    dateTime.Millisecond
                )
            );
        }
    }
}
