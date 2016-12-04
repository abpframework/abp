using System;
using Abp.Extensions;
using Shouldly;
using Xunit;

namespace Abp.Tests.Extensions
{
    public class DayOfWeekExtensions_Tests
    {
        [Fact]
        public void Weekend_Weekday_Test()
        {
            DayOfWeek.Monday.IsWeekday().ShouldBe(true);
            DayOfWeek.Monday.IsWeekend().ShouldBe(false);

            DayOfWeek.Saturday.IsWeekend().ShouldBe(true);
            DayOfWeek.Saturday.IsWeekday().ShouldBe(false);

            var datetime1 = new DateTime(2014, 10, 5, 16, 37, 25); //Sunday
            var datetime2 = new DateTime(2014, 10, 7, 16, 37, 25); //Tuesday

            datetime1.DayOfWeek.IsWeekend().ShouldBe(true);
            datetime2.DayOfWeek.IsWeekend().ShouldBe(false);

            datetime1.DayOfWeek.IsWeekday().ShouldBe(false);
            datetime2.DayOfWeek.IsWeekday().ShouldBe(true);
        }
    }
}