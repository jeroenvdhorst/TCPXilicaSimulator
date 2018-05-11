using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace TCPXilicaSimulator
{
    public class Packet
    {
        public static int GetLengthOfPacket(string buffer)
        {
            if (buffer.Length < 4) return -1;
            //Continue means: if _totalBuffer.Lenght < 4, DO NOT PROCEED
            return int.Parse(buffer.Substring(0, 4));
        }

        public static int GetLengthOfPacket(List<byte> buffer)
        {
            if (buffer.Count < 4) return -1;
            var t = BitConverter.ToInt32(buffer.ToArray(), 0);
            return t;
        }

        /// <summary>
        ///  Tries to retrieve exactly one packet as a JSON object from a byte list.
        /// </summary>
        public static string RetriveString(int packetSize, ref List<byte> buffer)
        {
            return Encoding.UTF8.GetString(GetPacketBytes(packetSize, ref buffer).ToArray());
        }

        private static List<byte> GetPacketBytes(int packetSize, ref List<byte> buffer)
        {
            var data = buffer.GetRange(4, packetSize - 4);
            buffer.RemoveRange(0, packetSize + 4);
            return data;
        }

        /// <summary>
        ///  Creates a byte array from the specified string. First four bytes contains the length the data. The remainder of the bytes is the data bytes created from the given string.
        /// </summary>
        public static byte[] CreateByteData(string s)
        {
            var bytes = Encoding.UTF8.GetBytes(s);
            var length = BitConverter.GetBytes(bytes.Length);
            var data = length.Concat(bytes).ToArray();
            return data;
        }
    }
}
