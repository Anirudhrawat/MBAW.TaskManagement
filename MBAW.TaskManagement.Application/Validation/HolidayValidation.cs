namespace MBAW.TaskManagement.Application.Validation
{
    public class HolidayValidation
    {
        static DateTime NewYear(int year) => FixWeekendHoliday(new DateTime(year, 1, 1));
        static DateTime MartinLutherKing(int year) => FindNthDayOfMonth(year, 1, DayOfWeek.Monday, 3);
        static DateTime PresidentsDay(int year) => FindNthDayOfMonth(year, 2, DayOfWeek.Monday, 3);
        static DateTime MemorialDay(int year) => FindLastDayOfMonth(year, 5, DayOfWeek.Monday);
        static DateTime Juneteenth(int year) => FixWeekendHoliday(new DateTime(year, 6, 19));
        static DateTime IndependenceDay(int year) => FixWeekendHoliday(new DateTime(year, 7, 4));
        static DateTime LaborDay(int year) => FindNthDayOfMonth(year, 9, DayOfWeek.Monday, 1);
        static DateTime ColumbusDay(int year) => FindNthDayOfMonth(year, 10, DayOfWeek.Monday, 2);
        static DateTime VeteransDay(int year) => FixWeekendHoliday(new DateTime(year, 11, 11));
        static DateTime Thanksgiving(int year) => FindNthDayOfMonth(year, 11, DayOfWeek.Thursday, 4);
        static DateTime Christmas(int year) => FixWeekendHoliday(new DateTime(year, 12, 25));

        public static DateTime FixWeekendHoliday(DateTime holiday)
        {
            if (holiday.DayOfWeek == DayOfWeek.Saturday)
            {
                return holiday.AddDays(-1);
            }
            if (holiday.DayOfWeek == DayOfWeek.Sunday)
            {
                return holiday.AddDays(1);
            }
            return holiday;
        }

        public static DateTime FindNthDayOfMonth(int year, int month, DayOfWeek dayOfWeek, int occurrence)
        {
            DateTime firstDay = new DateTime(year, month, 1);
            int offset = (int)dayOfWeek - (int)firstDay.DayOfWeek;
            if (offset < 0) offset += 7;
            return firstDay.AddDays(offset + 7 * (occurrence - 1));
        }

        public static DateTime FindLastDayOfMonth(int year, int month, DayOfWeek dayOfWeek)
        {
            var lastDay = new DateTime(year, month, DateTime.DaysInMonth(year, month));
            int offset = (int)lastDay.DayOfWeek - (int)dayOfWeek;
            if (offset < 0) offset += 7;
            return lastDay.AddDays(-offset);
        }
        public static bool IsValidDueDate(DateTime date)
        {
            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
            {
                return false;
            }
            if (IsFederalHoliday(date))
            {
                return false;
            }
            return true;
        }

        public static bool IsFederalHoliday(DateTime date)
        {
            int year = date.Year;
            var holidays = new List<DateTime>
            {
                NewYear(year),
                MartinLutherKing(year),
                PresidentsDay(year),
                MemorialDay(year),
                Juneteenth(year),
                IndependenceDay(year),
                LaborDay(year),
                ColumbusDay(year),
                VeteransDay(year),
                Thanksgiving(year),
                Christmas(year)
            };
            return holidays.Contains(date.Date);
        }
    }
}
