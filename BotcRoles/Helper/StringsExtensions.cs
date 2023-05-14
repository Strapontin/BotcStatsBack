using System.Globalization;
using System.Text;

namespace BotcRoles.Helper
{
    public static class StringsExtensions
    {
        public static string RemoveDiacritics(this string str)
        {
            var normalizedString = str.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder(capacity: normalizedString.Length);

            for (int i = 0; i < normalizedString.Length; i++)
            {
                char c = normalizedString[i];
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder
                .ToString()
                .Normalize(NormalizationForm.FormC);
        }

        public static string ToLowerRemoveDiacritics(this string str)
        {
            return str.RemoveDiacritics().ToLower();
        }
    }
}
