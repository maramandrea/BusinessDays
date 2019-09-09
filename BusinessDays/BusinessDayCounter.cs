using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessDays
{
    public class BusinessDayCounter
    {
        /// <summary>
        /// Calculates the weekdays between two given dates excluding the start and end date.
        /// </summary>
        /// <param name="firstDate"></param>
        /// <param name="secondDate"></param>
        /// <returns></returns>
        public int WeekdaysBetweenTwoDates(DateTime firstDate, DateTime secondDate)
        {
            var results = 0;

            if (!AreDatesValid(firstDate, secondDate))
                return results;

            // We don't want to include the first date, so start with the next day.
            var currentDate = firstDate.AddDays(1);

            while (currentDate != secondDate)
            {
                if (!IsWeekend(currentDate))
                    results++;

                currentDate = currentDate.AddDays(1);
            }

            return results;
        }

        /// <summary>
        /// Calculates the business days between two given days excluding the start and end date.
        /// </summary>
        /// <param name="firstDate"></param>
        /// <param name="secondDate"></param>
        /// <param name="publicHolidays">List of public holidays in simple DateTime.</param>
        /// <returns></returns>
        public int BusinessDaysBetweenTwoDates(DateTime firstDate, DateTime secondDate, IList<DateTime> publicHolidays)
        {
            var results = 0;

            if (!AreDatesValid(firstDate, secondDate))
                return results;

            // We don't want to include the first date, so start with the next day.
            var currentDate = firstDate.AddDays(1);

            while (currentDate != secondDate)
            {
                if (!IsWeekend(currentDate) && !IsHoliday(currentDate, publicHolidays))
                    results++;

                currentDate = currentDate.AddDays(1);
            }

            return results;
        }

        /// <summary>
        /// Calculates the business days between two given days excluding the start and end date.
        /// </summary>
        /// <param name="firstDate"></param>
        /// <param name="secondDate"></param>
        /// <param name="publicHolidays">List of public holidays using Holiday to define the holidays.</param>
        /// <returns></returns>
        public int BusinessDaysBetweenTwoDates(DateTime firstDate, DateTime secondDate, IList<Holiday> publicHolidays)
        {
            var results = 0;

            if (!AreDatesValid(firstDate, secondDate))
                return results;

            // We don't want to include the first date, so start with the next day.
            var currentDate = firstDate.AddDays(1);

            while (currentDate != secondDate)
            {
                if (!IsWeekend(currentDate) && !IsHoliday(currentDate, publicHolidays))
                    results++;

                currentDate = currentDate.AddDays(1);
            }

            return results;
        }

        private bool AreDatesValid(DateTime firstDate, DateTime secondDate)
        {
            return secondDate <= firstDate ? false : true;
        }

        private bool IsWeekend(DateTime date)
        {
            return date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;
        }

        private bool IsHoliday(DateTime date, IList<DateTime> holidays)
        {
            return holidays.Contains(date);
        }

        private bool IsHoliday(DateTime date, IList<Holiday> holidays)
        {
            // We should only be concerned about any holidays on the current month
            var holidaysForMonth = holidays.Where(x => x.Month == date.Month).ToList();

            // If there are no holidays for this current month, then it is not a public holiday
            if (!holidaysForMonth.Any()) return false;

            foreach(var holiday in holidaysForMonth)
            {
                if (holiday is OccurenceHoliday occurence)
                {
                    var dateForHoliday = occurence.GetHolidayDate(date.Year);

                    if (date == dateForHoliday)
                        return true;

                }
                else
                {
                    var sameDayHoliday = (SameDayHoliday)holiday;
                    var holidayDate = new DateTime(date.Year, sameDayHoliday.Month, sameDayHoliday.Day);

                    if (sameDayHoliday.IsObserved && IsWeekend(holidayDate))
                    {
                        holidayDate = sameDayHoliday.GetHolidayDate(date.Year);
                    }

                    if (date == holidayDate)
                        return true;
                }
            }
            return false;
        }
    }
}
