using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Enumerables;
using SDG.Unturned;

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
                return hash_sha1(text);
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

        private static int derecogText(string text)
        {
            byte[] bytes_text = Encoding.Unicode.GetBytes(text);
            int calculator = 0;

            for (int i = 0; i < bytes_text.Length; i++)
                if (bytes_text[i] % 2 == 0)
                    calculator += (int)bytes_text[i];
                else
                    calculator -= (int)bytes_text[i];
            if (calculator < 0)
                calculator = -calculator;
            else if (calculator == 0)
                calculator = text.Length;

            return calculator;
        }
        #endregion

        #region SHA256 Functions
        #endregion

        #region SHA1 Functions
        private static string hash_sha1(string text)
        {
            return Encoding.Unicode.GetString(Hash.SHA1(text)).Replace('-', '\0');
        }
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
        private static string enc_unp_encrypt(string text, string username, string password)
        {
            string result = text;

            result = unp_apply_username(result, username);
            result = unp_apply_password(result, password);
            result = shift_right(result, remedyText(hash_sha1(username + password)));

            return result;
        }

        private static string enc_unp_decrypt(string text, string username, string password)
        {

        }

        private static string unp_apply_username(string text, string username)
        {
            byte[] bytes_text = Encoding.Unicode.GetBytes(text);
            byte[] bytes_username = Encoding.Unicode.GetBytes(username);
            int calculator = derecogText(username);

            for (int a = 0; a < bytes_text.Length; a++)
            {
                bytes_text[a] *= (byte)calculator;
                for (int b = 0; b < bytes_username.Length; b++)
                    if (bytes_username[b] % 2 == 0)
                        bytes_text[a] += bytes_username[b];
                    else
                        bytes_text[a] -= bytes_username[b];
            }

            return Encoding.Unicode.GetString(bytes_text);
        }

        private static string unp_apply_password(string text, string password)
        {
            string passHash = hash_sha1(password);
            string result = "";
            int rText = remedyText(password);
            Random r = new Random();

            for (int a = 0; a < text.Length; a++)
            {
                if (text[a] + rText % 2 == 0)
                    result += passHash[r.Next(0, passHash.Length)];
                result += text[a];
            }

            return result;
        }
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
