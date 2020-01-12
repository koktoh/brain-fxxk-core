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

        public static void Save<T>(T source, string path)
        {
            var fi = new FileInfo(path);
            Save(source, fi);
        }

        public static void Save<T>(T source, FileInfo file)
        {
            var config = ConfigSerializer.Serialize(source);
            var formatted = ConfigSerializer.Format(config);

            using var sw = file.CreateText();
            sw.WriteLine(formatted);
        }
    }
}
