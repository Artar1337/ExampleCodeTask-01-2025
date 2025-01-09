using UnityEngine;
using System.Security.Cryptography;
using System.Text;

namespace Data
{
    /// <summary>
    /// Minimally protected from cheating player prefs
    /// https://habr.com/ru/sandbox/92973/
    /// </summary>
    public static class CryptographicPlayerPrefs
    {
        private const string SecretKey = "TestTaskAstrakhantsev-01/2025";
        private static readonly byte[] Key = new byte[8] { 32, 21, 56, 147, 66, 231, 43, 43 };
        private static readonly byte[] Iv = new byte[8] { 55, 78, 21, 67, 55, 86, 99, 1 };

        public static void SetString(string key, string value)
        {
            PlayerPrefs.SetString(MD5(key), Encrypt(value));
        }

        public static string GetString(string key, string defaultValue)
        {
            if (!HasKey(key))
            {
                return defaultValue;
            }  
            try
            {
                string s = Decrypt(PlayerPrefs.GetString(MD5(key)));
                return s;
            }
            catch
            {
                return defaultValue;
            }
        }

        public static string GetString(string key)
        {
            return GetString(key, string.Empty);
        }

        public static void SetInt(string key, int value)
        {
            PlayerPrefs.SetString(MD5(key), Encrypt(value.ToString()));
        }

        public static int GetInt(string key, int defaultValue)
        {
            if (!HasKey(key))
            {
                return defaultValue;
            } 
            try
            {
                string s = Decrypt(PlayerPrefs.GetString(MD5(key)));
                int i = int.Parse(s);
                return i;
            }
            catch
            {
                return defaultValue;
            }
        }

        public static int GetInt(string key)
        {
            return GetInt(key, 0);
        }


        public static void SetFloat(string key, float value)
        {
            PlayerPrefs.SetString(MD5(key), Encrypt(value.ToString()));
        }


        public static float GetFloat(string key, float defaultValue)
        {
            if (!HasKey(key))
                return defaultValue;
            try
            {
                string s = Decrypt(PlayerPrefs.GetString(MD5(key)));
                float f = float.Parse(s, System.Globalization.CultureInfo.InvariantCulture);
                return f;
            }
            catch
            {
                return defaultValue;
            }
        }

        public static float GetFloat(string key)
        {
            return GetFloat(key, 0);
        }

        public static bool HasKey(string key)
        {
            return PlayerPrefs.HasKey(MD5(key));
        }

        public static void DeleteKey(string key)
        {
            PlayerPrefs.DeleteKey(MD5(key));
        }

        public static void Save()
        {
            PlayerPrefs.Save();
        }

        private static string Encrypt(string s)
        {
            byte[] inputbuffer = Encoding.Unicode.GetBytes(s);
            byte[] outputBuffer = DES.Create().CreateEncryptor(Key, Iv).TransformFinalBlock(inputbuffer, 0, inputbuffer.Length);
            return System.Convert.ToBase64String(outputBuffer);
        }

        private static string Decrypt(string s)
        {
            byte[] inputbuffer = System.Convert.FromBase64String(s);
            byte[] outputBuffer = DES.Create().CreateDecryptor(Key, Iv).TransformFinalBlock(inputbuffer, 0, inputbuffer.Length);
            return Encoding.Unicode.GetString(outputBuffer);
        }

        private static string MD5(string s)
        {
            byte[] hashBytes = new MD5CryptoServiceProvider().ComputeHash(new UTF8Encoding().GetBytes(s + SecretKey + SystemInfo.deviceUniqueIdentifier));
            string hashString = "";
            for (int i = 0; i < hashBytes.Length; i++)
            {
                hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
            }
            return hashString.PadLeft(32, '0');
        }
    }
}
