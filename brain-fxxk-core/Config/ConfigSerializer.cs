using System.IO;
using Newtonsoft.Json;

namespace BFCore.Config
{
    internal static class ConfigSerializer
    {
        internal static string Serialize<T>(T source)
        {
            return JsonConvert.SerializeObject(source);
        }

        internal static T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        internal static T Deserialize<T>(Stream stream)
        {
            using (var sr = new StreamReader(stream))
            {
                return JsonConvert.DeserializeObject<T>(sr.ReadToEnd());
            }
        }

        internal static string Format(string source)
        {
            return JsonConvert.SerializeObject(JsonConvert.DeserializeObject(source), Formatting.Indented);
        }
    }
}
