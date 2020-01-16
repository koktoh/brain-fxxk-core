using System;
using System.Collections.Generic;

namespace BFCore.Extesion
{
    public static class StringExtensions
    {
        public static bool IsNullOrEmpty(this string source)
        {
            return string.IsNullOrEmpty(source);
        }

        public static bool IsNullOrWhiteSpace(this string source)
        {
            return string.IsNullOrWhiteSpace(source);
        }

        public static bool HasValue(this string source)
        {
            return !source.IsNullOrEmpty();
        }

        public static bool HasMeaningfulValue(this string source)
        {
            return !source.IsNullOrWhiteSpace();
        }

        public static IEnumerable<string> SplitNewLine(this string source, StringSplitOptions options = StringSplitOptions.None)
        {
            return source.Split(new[] { Environment.NewLine }, options);
        }
    }
}
