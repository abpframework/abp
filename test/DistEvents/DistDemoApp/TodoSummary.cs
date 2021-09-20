using System;
using Volo.Abp.Domain.Entities;

namespace DistDemoApp
{
    public class TodoSummary : AggregateRoot<int>
    {
        public int Year { get; private set; }
        public byte Month { get; private set; }
        public byte Day { get; private set; }
        public int TotalCount { get; private set; }

        private TodoSummary()
        {
            
        }

        public TodoSummary(DateTime dateTime, int initialCount = 1)
        {
            Year = dateTime.Year;
            Month = (byte)dateTime.Month;
            Day = (byte)dateTime.Day;
            TotalCount = initialCount;
        }

        public void Increase(int amount = 1)
        {
            TotalCount += amount;
        }
        
        public void Decrease(int amount = 1)
        {
            TotalCount -= amount;
        }

        public override string ToString()
        {
            return $"{base.ToString()}, {Year}-{Month:00}-{Day:00}: {TotalCount}";
        }
    }
}