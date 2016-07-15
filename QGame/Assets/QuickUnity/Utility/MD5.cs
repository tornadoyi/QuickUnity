using UnityEngine;
using System.Collections;
using System.Text;
using System.Security.Cryptography;
using System;
using System.IO;

namespace Utility
{
    public class MD5
    {
        public static string Compute(byte[] bytes)
        {
            if (bytes == null) return string.Empty;
            var md5 = new MD5CryptoServiceProvider();
            byte[] data = md5.ComputeHash(bytes);
            var str = System.BitConverter.ToString(data);
            return Process(str);
        }

        public static string Compute(Stream stream)
        {
            if (stream == null) return string.Empty;
            var md5 = new MD5CryptoServiceProvider();
            byte[] data = md5.ComputeHash(stream);
            var str = System.BitConverter.ToString(data);
            return Process(str);
        }

        private static string Process(string md5)
        {
            if (string.IsNullOrEmpty(md5)) return string.Empty;
            return md5.ToLower().Replace("-", "");
        }
    }
}


