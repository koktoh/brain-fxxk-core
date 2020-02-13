using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace brain_fxxk_core.test
{
    // from: https://take4-blue.com/program/%E5%8D%98%E4%BD%93%E3%83%86%E3%82%B9%E3%83%88%E3%81%A7%E3%83%95%E3%82%A1%E3%82%A4%E3%83%AB%E3%82%92%E5%88%A9%E7%94%A8%E3%81%99%E3%82%8B/
    public static class FileAssert
    {
        public static void AreEqual(string fileA, string fileB)
        {
            CollectionAssert.AreEqual(File.ReadAllBytes(fileA), File.ReadAllBytes(fileB));
        }
    }
}
