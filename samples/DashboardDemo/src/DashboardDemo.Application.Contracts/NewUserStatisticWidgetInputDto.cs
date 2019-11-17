using System;

namespace DashboardDemo
{
    public class NewUserStatisticWidgetInputDto
    {
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public NewUserStatisticFrequency Frequency { get; set; } = NewUserStatisticFrequency.Daily;
    }
}