using System.Globalization;

namespace Competitions.Common.Helpers
{
    public static class DateTimeConvertor
    {
        public static String ToShamsi(this DateTime dateTime)
        {
            PersianCalendar pc = new PersianCalendar();
            return
                pc.GetYear(dateTime).ToString() + "/" +
                pc.GetMonth(dateTime).ToString("00") + '/' +
                pc.GetDayOfMonth(dateTime).ToString("00");
        }

        public static String ToShamsi(this String strDateTime)
        {
            if (String.IsNullOrWhiteSpace(strDateTime))
                return "";

            CultureInfo culture = new CultureInfo("en-US");
            PersianCalendar pc = new PersianCalendar();
            DateTime dateTime = Convert.ToDateTime(strDateTime, culture);

            return
                pc.GetYear(dateTime).ToString() + "/" +
                pc.GetMonth(dateTime).ToString("00") + '/' +
                pc.GetDayOfMonth(dateTime).ToString("00");
        }

        public static DateTime ShamsiYearBegin(this DateTime dateTime)
        {
            var pc = new PersianCalendar();
            int year = pc.GetYear(dateTime);
            return new DateTime(year, 1, 1, pc);
        }

        public static DateTime ShamsiYearEnd(this DateTime dateTime)
        {
            var pc = new PersianCalendar();

            int year = pc.GetYear(dateTime);
            int month = 12;
            int day = GetMonthDays(year, month);
            return new DateTime(year, month, day, pc);
        }

        public static DateTime GetTheLastDateOfTheMonth(this DateTime dateTime)
        {
            var pc = new PersianCalendar();
            int day = dateTime.GetMonthDays();
            int year = pc.GetYear(dateTime);
            int month = pc.GetMonth(dateTime);

            dateTime = new DateTime(year: year, month: month, day: day, calendar: pc);
            return dateTime;
        }

        public static DateTime ToMiladi(this DateTime dateTime)
        {
            if (dateTime != default)
            {
                PersianCalendar pc = new PersianCalendar();
                return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, pc);
            }

            return dateTime;
        }

        public static DateTime? ToMiladi(this DateTime? dateTime)
        {
            if (dateTime.HasValue)
            {
                PersianCalendar pc = new PersianCalendar();
                return new DateTime(dateTime.Value.Year, dateTime.Value.Month, dateTime.Value.Day, pc);
            }

            return dateTime;
        }

        public static String ToMiladi(this String strDateTime)
        {
            var pc = new PersianCalendar();
            if (String.IsNullOrWhiteSpace(strDateTime))
                return "";

            var parts = strDateTime.Split('/');

            if (parts.Length != 3)
                return "";

            DateTime dateTime = new DateTime(Convert.ToInt32(parts[0]), Convert.ToInt32(parts[1]), Convert.ToInt32(parts[2]), pc);

            return dateTime.ToString();
        }

        public static DateTime ToDateTime(this String strDateTime)
        {
            if (String.IsNullOrWhiteSpace(strDateTime))
                return default;

            DateTime dateTime = DateTime.Parse(strDateTime);

            return dateTime;
        }

        public static int GetShamsiYear(this DateTime dateTime)
        {
            PersianCalendar pc = new PersianCalendar();
            return pc.GetYear(dateTime);
        }

        public static String PersianDayOfWeek(this DateTime dateTime)
        {
            String day = dateTime.DayOfWeek.ToString();
            switch (day.ToLower())
            {
                case "monday": return "دو شنبه";
                case "tuesday": return "سه شنبه";
                case "wednesday": return "چهار شنبه";
                case "thursday": return "پنج شنبه";
                case "friday": return "جمعه";
                case "saturday": return "شنبه";
                case "sunday": return "یک شنبه";
                default: return day;
            }
        }

        public static int DayOfWeekNumber(this DateTime dateTime)
        {
            String day = dateTime.DayOfWeek.ToString();
            switch (day.ToLower())
            {
                case "monday": return 3;
                case "tuesday": return 4;
                case "wednesday": return 5;
                case "thursday": return 6;
                case "friday": return 7;
                case "saturday": return 1;
                case "sunday": return 2;
                default: return 0;
            }
        }

        public static String GetShamsiMonthName(this DateTime dateTime)
        {
            PersianCalendar pc = new PersianCalendar();
            var month = pc.GetMonth(dateTime).ToString();

            switch (month)
            {
                case "1": return "فروردین";
                case "2": return "اردیبهشت";
                case "3": return "خرداد";
                case "4": return "تیر";
                case "5": return "مرداد";
                case "6": return "شهریور";
                case "7": return "مهر";
                case "8": return "آبان";
                case "9": return "آذر";
                case "10": return "دی";
                case "11": return "بهمن";
                case "12": return "اسفند";
                default: return "نامشخص";
            }
        }

        public static int GetShamsiMonth(this DateTime dateTime)
        {
            var pc = new PersianCalendar();
            return pc.GetMonth(dateTime);
        }

        public static int GetMonthDays(this DateTime dateTime)
        {
            PersianCalendar pc = new PersianCalendar();
            int month = pc.GetMonth(dateTime);
            int year = pc.GetYear(dateTime);
            return GetMonthDays(year, month);
        }

        public static DateTime GetDateFromString(String date)
        {
            var parts = date.Split(' ').ToList();
            if (parts.Count() == 10)
                parts.Remove(parts[0]);


            int year = int.Parse(parts[3].ToEnglishNumbers());
            int month = GetShamsiMonthNumber(parts[2]);
            int day = int.Parse(parts[1].ToEnglishNumbers());

            int hour = int.Parse(parts[5].Split(':')[0].ToEnglishNumbers());
            int min = int.Parse(parts[5].Split(':')[1].ToEnglishNumbers());

            PersianCalendar pc = new PersianCalendar();

            return new DateTime(year, month, day, hour, min, 0, pc);
        }

        public static String GetWebToolKitString(this DateTime dateTime)
        {
            return dateTime.Year + "/" + dateTime.Month + "/" + dateTime.Day + " " + dateTime.TimeOfDay.ToString().Substring(0, 5);
        }


        private static int GetMonthDays(int year, int month)
        {

            if (month < 7)
                return 31;

            if (month < 12)
                return 30;

            if (LeapYear(year))
                return 30;

            return 29;
        }

        private static bool LeapYear(int year) => ((year % 4 == 0) && (year % 100 != 0)) || (year % 400 == 0);

        private static int GetShamsiMonthNumber(String month)
        {
            switch (month)
            {
                case "فروردین": return 1;
                case "اردیبهشت": return 2;
                case "خرداد": return 3;
                case "تیر": return 4;
                case "مرداد": return 5;
                case "شهریور": return 6;
                case "مهر": return 7;
                case "آبان": return 8;
                case "آذر": return 9;
                case "دی": return 10;
                case "بهمن": return 11;
                case "اسفند": return 12;
                default: return 0;
            }
        }

        private static string ToEnglishNumbers(this string input)
        {
            string[] persian = new string[10] { "۰", "۱", "۲", "۳", "۴", "۵", "۶", "۷", "۸", "۹" };

            for (int j = 0; j < persian.Length; j++)
                input = input.Replace(persian[j], j.ToString());

            return input;
        }
    }
}
