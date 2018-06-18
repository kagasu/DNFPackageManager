using System;
using System.IO;

namespace PackageManager
{
    public static class StreamExtensions
    {
        public static int Read(this Stream stream, byte[] buf) => stream.Read(buf, 0, buf.Length);

        public static int Read(this Stream stream, int length, out byte[] buf)
        {
            buf = new byte[length];
            return stream.Read(buf, 0, length);
        }

        public static int ReadInt(this Stream stream)
        {
            stream.Read(4, out byte[] buf);
            return BitConverter.ToInt32(buf, 0);
        }

        public static uint ReadUInt(this Stream stream)
        {
            stream.Read(4, out byte[] buf);
            return BitConverter.ToUInt32(buf, 0);
        }

        public static short ReadShort(this Stream stream)
        {
            var buf = new byte[2];
            stream.Read(buf, 0, buf.Length);
            return BitConverter.ToInt16(buf, 0);
        }

        public static ushort ReadUShort(this Stream stream)
        {
            var buf = new byte[2];
            stream.Read(buf, 0, buf.Length);
            return BitConverter.ToUInt16(buf, 0);
        }

        public static long ReadLong(this Stream stream)
        {
            var buf = new byte[8];
            stream.Read(buf, 0, buf.Length);
            return BitConverter.ToInt64(buf, 0);
        }

        public static byte[] ReadToEnd(this Stream stream)
        {
            var buf = new byte[stream.Length - stream.Position];
            stream.Read(buf, 0, buf.Length);
            return buf;
        }

        public static void ReadToEnd(this Stream stream, out byte[] buf)
        {
            buf = new byte[stream.Length - stream.Position];
            stream.Read(buf, 0, buf.Length);
        }

        public static void Write(this Stream stream, byte[] buf) => stream.Write(buf, 0, buf.Length);
    }
}