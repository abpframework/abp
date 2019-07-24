using System;

namespace DashboardDemo
{
    public class NewUserStatisticWidgetInputDto : WidgetFilter
    {
        public NewUserStatisticFrequency Frequency { get; set; } = NewUserStatisticFrequency.Daily;
    }
}