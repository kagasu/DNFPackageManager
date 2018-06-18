// Credit: https://github.com/Kritsu/ExtractorSharp
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ICSharpCode.SharpZipLib.BZip2;

namespace PackageManager
{
    // Token: 0x02000002 RID: 2
    public static class Spks
    {
        // Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
        public static byte[] Decompress(byte[] bs)
        {
            byte[] result;
            using (MemoryStream memoryStream = new MemoryStream(bs))
            {
                result = Spks.Decompress(memoryStream);
            }
            return result;
        }

        // Token: 0x06000003 RID: 3 RVA: 0x000020C0 File Offset: 0x000002C0
        public static byte[] Decompress(Stream stream)
        {
            stream.Seek(272, SeekOrigin.Begin);
            byte[] data;
            StreamExtensions.ReadToEnd(stream, out data);
            byte[][] array = data.Split(Spks.HEADER);
            byte[] result;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                for (int i = 1; i < array.Length; i++)
                {
                    byte[][] array2 = array[i].Split(Spks.MARK);
                    using (MemoryStream memoryStream2 = new MemoryStream(Arrays.Concat<byte>(Spks.HEADER, array2[0])))
                    {
                        BZip2.Decompress(memoryStream2, memoryStream, false);
                    }
                    if (array2.Length > 1)
                    {
                        for (int j = 1; j < array2.Length - 1; j++)
                        {
                            StreamExtensions.Write(memoryStream, array2[j].Sub(32));
                        }
                        byte[] array3 = array2.Last<byte[]>();
                        int num = array3.LastIndexof(Spks.TAIL);
                        num = ((num < 0) ? (num + array3.Length) : num);
                        StreamExtensions.Write(memoryStream, array3.Sub(32, num));
                    }
                }
                result = memoryStream.ToArray();
            }
            return result;
        }

        // Token: 0x06000004 RID: 4 RVA: 0x000021DC File Offset: 0x000003DC
        public static int LastIndexof(this byte[] data, byte[] pattern)
        {
            for (int i = data.Length - 1; i > 0; i--)
            {
                int num = i;
                while (data[num] == pattern[num - i])
                {
                    num++;
                    if (num - i == pattern.Length)
                    {
                        return i;
                    }
                }
            }
            return -1;
        }

        // Token: 0x06000005 RID: 5 RVA: 0x00002216 File Offset: 0x00000416
        public static byte[] Sub(this byte[] array, int start)
        {
            return array.Sub(start, array.Length);
        }

        // Token: 0x06000006 RID: 6 RVA: 0x00002224 File Offset: 0x00000424
        public static byte[] Sub(this byte[] array, int start, int end)
        {
            byte[] array2 = new byte[end - start];
            Buffer.BlockCopy(array, start, array2, 0, end - start);
            return array2;
        }

        // Token: 0x06000007 RID: 7 RVA: 0x00002248 File Offset: 0x00000448
        public static byte[][] Split(this byte[] data, byte[] pattern)
        {
            int i = 0;
            List<byte[]> list = new List<byte[]>();
            int start = 0;
            while (i < data.Length)
            {
                i = data.Indexof(pattern, start);
                i = ((i == -1) ? data.Length : i);
                list.Add(data.Sub(start, i));
                if (i < data.Length)
                {
                    i = (start = i + pattern.Length);
                }
            }
            return list.ToArray();
        }

        // Token: 0x06000008 RID: 8 RVA: 0x000022A0 File Offset: 0x000004A0
        public static int Indexof(this byte[] data, byte[] pattern, int start)
        {
            for (int i = start; i < data.Length; i++)
            {
                int num = i;
                while (num < data.Length && data[num] == pattern[num - i])
                {
                    num++;
                    if (num - i == pattern.Length)
                    {
                        return num - pattern.Length;
                    }
                }
            }
            return -1;
        }

        // Token: 0x06000009 RID: 9 RVA: 0x000022E2 File Offset: 0x000004E2
        static Spks()
        {
            // Note: this type is marked as 'beforefieldinit'.
            byte[] array = new byte[4];
            array[0] = 1;
            Spks.TAIL = array;
        }

        // Token: 0x04000001 RID: 1
        private static byte[] HEADER = new byte[]
        {
            66,
            90,
            104,
            57,
            49,
            65,
            89,
            38,
            83,
            89
        };

        // Token: 0x04000002 RID: 2
        private static byte[] MARK = new byte[]
        {
            0,
            0,
            0,
            0,
            0,
            16,
            14,
            0,
            byte.MaxValue,
            byte.MaxValue,
            byte.MaxValue,
            byte.MaxValue,
            byte.MaxValue,
            239,
            241,
            byte.MaxValue
        };

        // Token: 0x04000003 RID: 3
        private static byte[] TAIL;
    }
}
