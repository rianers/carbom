using System.Globalization;

namespace CarBom.Utils
{
    public static class DateFormatterUtil
    {
        /// <summary>
        /// Generates the data accordingly to CARBOM pattern: weekDay day month year - Ex: Sex 29 julho 2022
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string FormatDateToCarBomPattern(DateTime date)
        {
            DateTimeFormatInfo cultureInfo = CultureInfo.GetCultureInfo("pt-BR").DateTimeFormat;
            string weekDay = cultureInfo.GetDayName(date.DayOfWeek);
            string day = date.Day.ToString();
            string month = cultureInfo.GetMonthName(date.Month);
            string year = date.Year.ToString();

            return string.Format("{0} {1} {2} {3}", weekDay, day, month, year);
        }

        /// <summary>
        /// Generates the date accordingly to ISO 8601 pattern (dd/MM/yyyy) format
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string FormatDateToISO8601Pattern(DateTime date) => date.ToString("dd/MM/yyyy");
    }
}
