using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace BFCore.Extesion
{
    public static class RegexExtensions
    {
        public static string Remove(this string source, string pattern)
        {
            return Regex.Replace(source, pattern, string.Empty);
        }

        public static string Replace(this string source, string pattern, string replacement)
        {
            return Regex.Replace(source, pattern, replacement);
        }

        public static bool IsMatch(this string source, string pattern)
        {
            return Regex.IsMatch(source, pattern);
        }

        public static bool IsMatch(this char source, string pattern)
        {
            return Regex.IsMatch(source.ToString(), pattern);
        }

        public static bool IsMatch(this IEnumerable<char> source, string pattern)
        {
            return Regex.IsMatch(source.Join(), pattern);
        }
    }
}
