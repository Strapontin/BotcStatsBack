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
                v => v.ToString(),
                v => DateTime.Parse(v));
        }
    }
}
