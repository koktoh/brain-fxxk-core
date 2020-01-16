using System.IO;

namespace BFCore.Config
{
    public static class ConfigManager
    {
        public static T Import<T>(string path)
        {
            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                return Import<T>(fs);
            }
        }

        public static T Import<T>(Stream stream)
        {
            return ConfigSerializer.Deserialize<T>(stream);
        }

        public static void Save<T>(T source, string path)
        {
            using (var fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
            {
                Save(source, fs);
            }
        }

        public static void Save<T>(T source, Stream stream)
        {
            var config = ConfigSerializer.Serialize(source);
            var formatted = ConfigSerializer.Format(config);

            using (var sw = new StreamWriter(stream))
            {
                sw.WriteLine(formatted);
            }
        }
    }
}
