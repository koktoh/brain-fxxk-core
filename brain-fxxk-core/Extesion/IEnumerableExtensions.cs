using System;
using System.Collections.Generic;
using System.Linq;

namespace BFCore.Extesion
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<T> SkipFirst<T>(this IEnumerable<T> source)
        {
            return source.Skip(1);
        }

        public static IEnumerable<T> SkipLast<T>(this IEnumerable<T> source)
        {
            var count = source.Count();

            for (int i = 0; i < count - 1; i++)
            {
                yield return source.ElementAt(i);
            }
        }

        public static IEnumerable<T> SkipFirstAndLast<T>(this IEnumerable<T> source)
        {
            return source.SkipFirst().SkipLast();
        }

        public static string Join(this IEnumerable<string> source, string separator)
        {
            return string.Join(separator, source);
        }

        public static string Join(this IEnumerable<string> source)
        {
            return source.Join(string.Empty);
        }

        public static string JoinNewLine(this IEnumerable<string> source)
        {
            return source.Join(Environment.NewLine);
        }

        public static string Join(this IEnumerable<char> source, string separator)
        {
            return string.Join(separator, source);
        }

        public static string Join(this IEnumerable<char> source)
        {
            return source.Join(string.Empty);
        }

        public static string JoinNewLine(this IEnumerable<char> source)
        {
            return source.Join(Environment.NewLine);
        }
    }
}
