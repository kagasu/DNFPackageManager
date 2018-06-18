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
            // IWebProxy proxy = new WebProxy("127.0.0.1", 8008);

            // Parse from bytes
            var baseUrl = "http://webdown2.nexon.co.jp/arad/real";
            var parser = new PackageParser(proxy);

            var bytes = await parser.DownloadPackageListData(baseUrl);
            using (var stream = new MemoryStream(bytes))
            {
                var list = parser.Parse(stream);
                File.WriteAllText("out.json", JsonConvert.SerializeObject(list, Formatting.Indented));
            }

            // Parse from file
            using (var stream = File.OpenRead("package.lst"))
            {
                var list = parser.Parse(stream);
                File.WriteAllText("out.json", JsonConvert.SerializeObject(list, Formatting.Indented));
            }
        }
    }
}
