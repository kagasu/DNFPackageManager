using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;
using System.Net;
using System.Text;
using System.Collections.Generic;
using PackageManager;

namespace Test
{
    class Program
    {
        static async Task Main(string[] args)
        {
            IWebProxy proxy = null;
            // for debug
            //IWebProxy proxy = new WebProxy("127.0.0.1", 8008);

            // Parse from bytes
            var baseUrl = "http://webdown2.nexon.co.jp/arad/real";
            var parser = new PackageManager.PackageManager(proxy);

            var bytes = await parser.DownloadPackageListData(baseUrl);
            using (var stream = new MemoryStream(bytes))
            {
                var packageInfo = parser.Parse(stream);
                File.WriteAllText("out.json", JsonConvert.SerializeObject(packageInfo, Formatting.Indented));

                // download all .npk files
                var tasks = packageInfo
                    .Where(x => x.DirectoryName == "ImagePacks2")
                    .SelectMany(x => x.Files)
                    .Select(x => parser.DownloadSPKFile($"{baseUrl}/ImagePacks2/{x.FileName}.spk", $"out/{x.FileName}"))
                    .ToArray();
                Task.WaitAll(tasks);
            }
            return;

            // Parse from file
            using (var stream = File.OpenRead("package.lst"))
            {
                var packageInfo = parser.Parse(stream);
                File.WriteAllText("out.json", JsonConvert.SerializeObject(packageInfo, Formatting.Indented));
            }
        }
    }
}
