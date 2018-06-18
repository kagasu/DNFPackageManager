using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;
using System.Net;
using System.Text;
using System.Collections.Generic;

namespace PackageManager
{
    public class PackageManager
    {
        private IWebProxy Proxy { get; }

        public PackageManager(IWebProxy proxy = null)
        {
            Proxy = proxy;
        }

        public async Task<byte[]> DownloadPackageListData(string baseUrl)
        {
            var handler = new HttpClientHandler();
            if (Proxy != null)
            {
                handler.Proxy = Proxy;
                handler.UseProxy = true;
            }

            var client = new HttpClient(handler);
            var bytes = await client.GetByteArrayAsync($"{baseUrl}/package.lst");
            return bytes;
        }

        public async Task DownloadSPKFile(string url, string path)
        {
            var handler = new HttpClientHandler();
            if (Proxy != null)
            {
                handler.Proxy = Proxy;
                handler.UseProxy = true;
            }

            var client = new HttpClient(handler);
            var bytes = await client.GetByteArrayAsync(url);
            bytes = Spks.Decompress(bytes);

            File.WriteAllBytes(path, bytes);
        }

        public List<PackageInformation> Parse(Stream stream)
        {
            var dirInfos = new List<PackageInformation>();

            stream.Seek(0x28, SeekOrigin.Begin); // skip unknown headers

            while (true)
            {
                var dirInfo = new PackageInformation();

                byte[] directoryNameBytes = null;
                var directoryNameLength = stream.ReadInt();
                if (directoryNameLength > 0)
                {
                    stream.Read(directoryNameLength - 1, out directoryNameBytes);
                    stream.Seek(1, SeekOrigin.Current); // trim no needed '\0'
                }
                else
                {
                    if (stream.Position == 0x2C)
                    {
                        directoryNameBytes = Encoding.UTF8.GetBytes("root");
                        stream.Seek(4, SeekOrigin.Current);
                    }
                    else
                    {
                        // end of file
                        return dirInfos;
                    }
                }

                var directoryName = (directoryNameBytes != null) ?
                    Encoding.UTF8.GetString(directoryNameBytes) :
                    null;

                dirInfo.DirectoryName = directoryName;
                var itemLength = stream.ReadInt();
                dirInfo.Files = new List<PackageFilenformation>();

                for (var i = 0; i < itemLength; i++)
                {
                    var fileNameLength = stream.ReadInt();
                    stream.Read(fileNameLength - 1, out var fileNameBytes);
                    stream.Seek(1, SeekOrigin.Current); // trim no needed '\0'
                    var fileSize = stream.ReadInt();
                    var unknown1 = stream.ReadInt();
                    var hashLength = stream.ReadInt();
                    stream.Read(hashLength, out var hashBytes);

                    dirInfo.Files.Add(new PackageFilenformation
                    {
                        FileName = Encoding.UTF8.GetString(fileNameBytes),
                        Unknown01 = unknown1,
                        FileSize = fileSize,
                        Sha256HashValue = hashBytes
                    });
                }
                dirInfo.DirectoryCount = stream.ReadInt();
                dirInfos.Add(dirInfo);
            }
        }
    }
}
