using System;

namespace BusinessDays
{
    public class Holiday
    {
        /// <summary>
        /// The month of the holiday
        /// </summary>
        public int Month { get; set; }

        /// <summary>
        /// Name of the holiday
        /// </summary>
        public string Name { get; set; }
    }

    /// <summary>
    /// Holidays that usually happen on the same day, unless they're an observed holiday.
    /// </summary>
    public class SameDayHoliday : Holiday
    {
        /// <summary>
        /// Teh day of the holiday
        /// </summary>
        public int Day { get; set; }

        /// <summary>
        /// If the holiday falls on a weekend, it moves to the following monday.
        /// </summary>
        public bool IsObserved { get; set; }

        /// <summary>
        /// Gets the date for the given holiday for the given year. 
        /// Takes into account whether or not the holiday is observed.
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public DateTime GetHolidayDate(int year)
        {
            var date = new DateTime(year, this.Month, this.Day);

            if (!this.IsObserved) return date;

            while (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
            {
                date = date.AddDays(1);
            }

            return date;
        }
    }

    /// <summary>
    /// Holidays that happen on a certain day and week of a month.
    /// </summary>
    public class OccurenceHoliday: Holiday
    {
        /// <summary>
        /// The day of the week that the holiday falls on
        /// </summary>
        public DayOfWeek DayOfWeek { get; set; }

        /// <summary>
        /// The week of the month that the holiday falls on
        /// </summary>
        public int WeekInMonth { get; set; }

        /// <summary>
        /// Gets the date for the given holiday for the given year.
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public DateTime GetHolidayDate(int year)
        {
            var date = new DateTime(year, this.Month, 1);
            var weekCounter = 0;

            while (weekCounter != this.WeekInMonth)
            {
                if (date.DayOfWeek == this.DayOfWeek)
                {
                    weekCounter++;

                    // We have the correct day of the week, we can now incremenet weekly
                    if (weekCounter != this.WeekInMonth)
                    {
                        date = date.AddDays(7);
                    }

                    continue;
                }

                date = date.AddDays(1);
            }

            return date;
        }
    }
}
