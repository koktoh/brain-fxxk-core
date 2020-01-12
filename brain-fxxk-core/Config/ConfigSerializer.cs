using System.IO;
using Utf8Json;

namespace BFCore.Config
{
    internal static class ConfigSerializer
    {
        internal static string Serialize<T>(T source)
        {
            return JsonSerializer.ToJsonString(source);
        }

        internal static T Deserialize<T>(string json)
        {
            return JsonSerializer.Deserialize<T>(json);
        }

        internal static T Deserialize<T>(Stream stream)
        {
            return JsonSerializer.Deserialize<T>(stream);
        }

        internal static string Format(string source)
        {
            return JsonSerializer.PrettyPrint(source);
        }
    }
}
