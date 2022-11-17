﻿using System.Security.Cryptography;
using System.Text;

namespace CarBom.Utils
{
    public static class EncryptUtil
    {
        /// <summary>
        /// Encrypt any string to SHA256 HashAlgorithm Cryptography
        /// </summary>
        /// <param name="rawData"></param>
        /// <returns></returns>
        public static string EncryptToSha256Hash(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                //ComputeHash - generate byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                //Convert byte array to string
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
