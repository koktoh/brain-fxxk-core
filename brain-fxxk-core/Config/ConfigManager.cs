using System.IO;

namespace BFCore.Config
{
    public static class ConfigManager
    {
        public static T Import<T>(string path)
        {
            var fi = new FileInfo(path);
            return Import<T>(fi);
        }

        public static T Import<T>(FileInfo file)
        {
            return ConfigSerializer.Deserialize<T>(file.OpenRead());
        }

        public static T Import<T>(Stream stream)
        {
            return ConfigSerializer.Deserialize<T>(stream);
        }

        public static void Save<T>(T source, string path)
        {
            var fi = new FileInfo(path);
            Save(source, fi);
        }

        public static void Save<T>(T source, FileInfo file)
        {
            Save(source, file.OpenWrite());
        }

        public static void Save<T>(T source, Stream stream)
        {
            var config = ConfigSerializer.Serialize(source);
            var formatted = ConfigSerializer.Format(config);

            using var sw = new StreamWriter(stream);
            sw.WriteLine(formatted);
        }
    }
}
