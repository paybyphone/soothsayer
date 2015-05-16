using System.Text.RegularExpressions;

namespace omt.Infrastructure
{
    public static class RegularExpressionExtensions
    {
        public static bool Matches(this string value, string regularExpression)
        {
            return Regex.IsMatch(value, regularExpression);
        }
    }
}