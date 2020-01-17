using System.Text.RegularExpressions;

namespace BFCore.Extesion
{
    public static class RegexExtensions
    {
        public static bool IsMatch(this string source, string pattern)
        {
            return Regex.IsMatch(source, pattern);
        }

        public static bool IsMatch(this char source, string pattern)
        {
            return Regex.IsMatch(source.ToString(), pattern);
        }
    }
}
