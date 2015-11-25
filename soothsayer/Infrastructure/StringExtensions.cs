using System.Collections;
using System.Collections.Generic;
using System.Globalization;

namespace soothsayer.Infrastructure
{
    public static class StringExtensions
    {
		public static bool IsNullOrEmpty(this string value)
		{
			return string.IsNullOrEmpty(value);
		}

        public static string FormatWith(this string value, params object[] formatObjects)
        {
            return string.Format(value, formatObjects);
        }

        public static string FormatInvariant(this string s, params object[] args) {
            return string.Format(CultureInfo.InvariantCulture, s, args);
        }

        public static string Join(this ICollection<string> strings)
        {
            return string.Join(", ", strings);
        }
    }
}
