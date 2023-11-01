using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Globalization;
using System.Text;

namespace BotcRoles.Helper
{
    public static class Converter
    {
        public static ValueConverter GetConverterBoolToInt()
        {
            return new ValueConverter<bool, int>(
                v => v ? 1 : 0,
                v => v == 1);
        }

        public static ValueConverter GetConverterDateTimeToString()
        {
            return new ValueConverter<DateTime, string>(
                v => v.ToString("yyyy-MM-dd HH:mm:ss"),
                v => GetDateFromString(v));
        }

        private static DateTime GetDateFromString(string v)
        {
            try
            {
                int year = int.Parse(v.Substring(0, 4));
                int month = int.Parse(v.Substring(5, 2));
                int day = int.Parse(v.Substring(8, 2));
                int hour = int.Parse(v.Substring(11, 2));
                int minute = int.Parse(v.Substring(14, 2));
                int second = int.Parse(v.Substring(17, 2));

                return new DateTime(year, month, day, hour, minute, second);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error on Date conversion ! ", e.Message);
                return new DateTime();
            }
        }
    }
}

