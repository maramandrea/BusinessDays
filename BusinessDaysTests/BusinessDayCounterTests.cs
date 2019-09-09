using BusinessDays;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Tests
{
    public class BusinessDayCounterTests
    {
        private BusinessDayCounter counter = null;

        [SetUp]
        public void Setup()
        {
            counter = new BusinessDayCounter();
        }

        [Test]
        [TestCase("2013-10-7", "2013-10-9", 1)]
        [TestCase("2013-10-5", "2013-10-14", 5)]
        [TestCase("2013-10-7", "2014-1-1", 61)]
        [TestCase("2013-10-7", "2013-10-5", 0)]
        public void CalculateWeekdaysBetweenTwoDates(DateTime firstDate, DateTime secondDate, int expectedResults)
        {

            var results = counter.WeekdaysBetweenTwoDates(firstDate, secondDate);
            Assert.AreEqual(expectedResults, results);
        }

        [Test]
        [TestCase("2013-10-7", "2013-10-9", 1)]
        [TestCase("2013-12-24", "2013-12-27", 0)]
        [TestCase("2013-10-7", "2014-1-1", 59)]

        public void CalculateBusinessDaysBetweenTwoDates(DateTime firstDate, DateTime secondDate, int expectedResults)
        {
            var holidays = new List<DateTime>
            {
                new DateTime(2013, 12, 25),
                new DateTime(2013, 12, 26),
                new DateTime(2014, 1, 1)
            };

            //var results = counter.BusinessDaysBetweenTwoDates(firstDate, secondDate, holidays);
            var results = counter.BusinessDaysBetweenTwoDates(firstDate, secondDate, holidays);

            Assert.AreEqual(expectedResults, results);
        }

        [Test]
        [TestCase("2013-10-7", "2013-10-9", 1)]
        [TestCase("2013-12-24", "2013-12-27", 0)]
        [TestCase("2013-10-7", "2014-1-1", 59)]
        [TestCase("2016-12-31", "2017-1-4", 1)]
        [TestCase("2016-10-7", "2017-1-2", 59)]
        [TestCase("2017-06-1", "2017-06-30", 19)]

        public void BusinessDaysBetweenTwoDates(DateTime firstDate, DateTime secondDate, int expectedResults)
        {
            var holidays = new List<Holiday>
            {
                new SameDayHoliday
                {
                    Name = "Christmas",
                    Month = 12,
                    Day = 25,
                    IsObserved = false
                },
                new SameDayHoliday
                {
                    Name = "Boxing Day",
                    Month = 12,
                    Day = 26,
                    IsObserved = true
                },
                new SameDayHoliday
                {
                    Name = "New Year's Day",
                    Month = 1,
                    Day = 1,
                    IsObserved = true
                },
                new SameDayHoliday
                {
                    Name = "ANZAC Day",
                    Month = 4,
                    Day = 25,
                    IsObserved = false
                },
                new OccurenceHoliday
                {
                    Name = "Queen's Birthday",
                    Month = 6,
                    DayOfWeek = DayOfWeek.Monday,
                    WeekInMonth = 2
                }
            };

            var results = counter.BusinessDaysBetweenTwoDates(firstDate, secondDate, holidays);
            Assert.AreEqual(expectedResults, results);
        }
    }
}