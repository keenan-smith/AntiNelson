using System;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PBExporter
{
    class Program
    {
        private static string password = "Fr34kyIsASkid";

        [STAThread]
        static void Main(string[] args)
        {
            byte[] ba = File.ReadAllBytes(Directory.GetCurrentDirectory() + "\\..\\..\\..\\AntiNelson\\bin\\PointBlank.dll");
            Clipboard.SetText(compress(crypt(ba)));
        }

        public static string CalculateMD5Hash(string input)
        {
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }

            return sb.ToString();
        }

        public static byte[] crypt(byte[] codes)
        {
            byte[] pass = Encoding.Unicode.GetBytes(CalculateMD5Hash(password));
            byte[] result = new byte[codes.Length];

            for (int i = 0; i < codes.Length; i++)
            {
                byte cde = codes[i];
                foreach (byte bt in pass)
                    cde = (byte)(cde ^ bt);
                result[i] = cde;
            }

            return result;
        }

        public static byte[] decompress(String input)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (BufferedStream bs = new BufferedStream(new DeflateStream(new MemoryStream(Convert.FromBase64String(input)), CompressionMode.Decompress)))
                {
                    bs.CopyTo(ms);
                }

                return ms.ToArray();
            }
        }

        public static String compress(byte[] input)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (BufferedStream bs = new BufferedStream(new DeflateStream(ms, CompressionMode.Compress)))
                {
                    bs.Write(input, 0, input.Length);
                }

                return Convert.ToBase64String(ms.ToArray());
            }
        }
    }
}
