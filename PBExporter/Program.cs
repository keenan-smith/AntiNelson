using System;
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

        [STAThread]
        static void Main(string[] args)
        {

            byte[] ba = File.ReadAllBytes(Directory.GetCurrentDirectory() + "\\..\\..\\..\\AntiNelson\\bin\\PointBlank.dll");
            Clipboard.SetText(compress(ba));

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
