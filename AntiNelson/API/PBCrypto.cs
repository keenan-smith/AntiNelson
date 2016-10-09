using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Enumerables;

namespace PointBlank.API
{
    public class PBCrypto
    {
        #region Functions
        public static string encrypt(string text, ECryptoType cryptoType, string password = null, string username = null)
        {
            if (string.IsNullOrEmpty(text))
                return null;

            if (cryptoType == ECryptoType.ENC_BACKLASH)
            {
                return enc_backlash_crypto(text);
            }
            else if (cryptoType == ECryptoType.ENC_POINTER)
            {
                if (string.IsNullOrEmpty(password))
                    return null;
                return enc_pointer_crypt(text, password, true);
            }
            else if (cryptoType == ECryptoType.ENC_SHIFT)
            {
                if (string.IsNullOrEmpty(password))
                    return null;
                return enc_shift_encrypt(text, password);
            }
            else if (cryptoType == ECryptoType.ENC_UNP)
            {

            }
            else if (cryptoType == ECryptoType.HASH_CHARPOS)
            {

            }
            else if (cryptoType == ECryptoType.HASH_MD5)
            {

            }
            else if (cryptoType == ECryptoType.HASH_SHA1)
            {

            }
            else if (cryptoType == ECryptoType.HASH_SHA256)
            {

            }
            else
            {
                return null;
            }
        }

        public static string decrypt(string text, ECryptoType cryptoType, string password = null, string username = null)
        {
            if (string.IsNullOrEmpty(text))
                return null;

            if (cryptoType == ECryptoType.ENC_BACKLASH)
            {
                return enc_backlash_crypto(text);
            }
            else if (cryptoType == ECryptoType.ENC_POINTER)
            {
                if (string.IsNullOrEmpty(password))
                    return null;
                return enc_pointer_crypt(text, password, false);
            }
            else if (cryptoType == ECryptoType.ENC_SHIFT)
            {
                if (string.IsNullOrEmpty(password))
                    return null;
                return enc_shift_decrypt(text, password);
            }
            else if (cryptoType == ECryptoType.ENC_UNP)
            {

            }
            else if (cryptoType == ECryptoType.HASH_CHARPOS)
            {

            }
            else if (cryptoType == ECryptoType.HASH_MD5)
            {

            }
            else if (cryptoType == ECryptoType.HASH_SHA1)
            {

            }
            else if (cryptoType == ECryptoType.HASH_SHA256)
            {

            }
            else
            {
                return null;
            }
        }

        private static int remedyText(string text)
        {
            byte[] bytes_text = Encoding.Unicode.GetBytes(text);
            int calculator1 = 0;
            int calculator2 = 0;

            Array.Sort(bytes_text, (a, b) => a.CompareTo(b));
            for (int i = 0; i < Math.Floor((double)(bytes_text.Length / 2)); i++)
            {
                calculator1 += (int)bytes_text[i];
                calculator2 += (int)bytes_text[(bytes_text.Length - 1) - i];
            }
            return calculator1 - calculator2;
        }
        #endregion

        #region SHA256 Functions
        #endregion

        #region SHA1 Functions
        #endregion

        #region MD5 Functions
        #endregion

        #region CHARPOS Functions
        #endregion

        #region SHIFT Functions
        private static string enc_shift_encrypt(string text, string password)
        {
            string result = text;

            foreach (char c in password)
            {
                int i = (int)c;

                if (i % 2 == 0)
                    result = shift_right(result, i);
                else
                    result = shift_left(result, i);
            }

            return result;
        }

        private static string enc_shift_decrypt(string text, string password)
        {
            string result = "";

            foreach (char c in password)
            {
                int i = (int)c;

                if (i % 2 == 0)
                    result = shift_left(result, i);
                else
                    result = shift_right(result, i);
            }

            return result;
        }

        private static string shift_right(string text, int steps)
        {
            string result = text;

            for (int a = 0; a < steps; a++)
            {
                result = result.Substring(result.Length - 1, 1) + result.Substring(0, result.Length - 1);
            }

            return result;
        }

        private static string shift_left(string text, int steps)
        {
            string result = text;

            for (int a = 0; a < steps; a++)
            {
                result = text.Substring(1, text.Length - 1) + text.Substring(0, 1);
            }

            return result;
        }
        #endregion

        #region UNP Functions
        #endregion

        #region POINTER Functions
        private static string enc_pointer_crypt(string text, string password, bool encrypt)
        {
            string result = "";
            int calculator = remedyText(password);
            
            foreach (char c in text)
            {
                result += (char)(encrypt ? ((int)c + calculator) : ((int)c - calculator));
            }

            return result;
        }
        #endregion

        #region BACKLASH Functions
        private static string enc_backlash_crypto(string text)
        {
            int backsize = backlash_getLargestChar(text);
            string result = "";

            foreach (char c in text)
            {
                if ((int)c >= backsize)
                    continue;

                result += (char)(backsize - c);
            }

            return result;
        }

        private static int backlash_getLargestChar(string text)
        {
            int charSize = 0;

            foreach (char c in text)
                if ((int)c > charSize)
                    charSize = (int)c;

            return charSize;
        }
        #endregion
    }
}
