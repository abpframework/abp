using Shouldly;
using Xunit;

namespace System
{
    public class ComparableExtensions_Tests
    {
        [Fact]
        public void IsBetween_Test()
        {
            //Number
            var number = 5;
            number.IsBetween(1, 10).ShouldBe(true);
            number.IsBetween(1, 5).ShouldBe(true);
            number.IsBetween(5, 10).ShouldBe(true);
            number.IsBetween(10, 20).ShouldBe(false);

            //DateTime
            var dateTimeValue = new DateTime(2014, 10, 4, 18, 20, 42, 0);
            dateTimeValue.IsBetween(new DateTime(2014, 1, 1), new DateTime(2015, 1, 1)).ShouldBe(true);
            dateTimeValue.IsBetween(new DateTime(2015, 1, 1), new DateTime(2016, 1, 1)).ShouldBe(false);
        }
    }
}