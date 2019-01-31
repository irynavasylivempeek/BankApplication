using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace BankApp.Utils
{
    public static class SaltedHashGenerator
    {
        private const int SaltSizeInBytes = 16;
        private static readonly HashAlgorithm _md5 = new MD5CryptoServiceProvider();

        public static byte[] GenerateSalt()
        {
            byte[] salt = new byte[SaltSizeInBytes];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }

        private static byte[] MergeByteArrays(byte[] byteArray1, byte[] byteArray2)
        {
            byte[] newByteArray = new byte[byteArray1.Length + byteArray2.Length];

            for (int i = 0; i < byteArray1.Length; i++)
                newByteArray[i] = byteArray1[i];

            for (int i = 0; i < byteArray2.Length; i++)
                newByteArray[byteArray1.Length + i] = byteArray2[i];
            return newByteArray;
        }

        public static string GenerateSaltedHash(string password, byte[] saltBytes)
        {
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] hashBytes = _md5.ComputeHash(passwordBytes);
            byte[] hashWithSaltBytes = MergeByteArrays(hashBytes, saltBytes);
            string resultHashWithSalt = Convert.ToBase64String(_md5.ComputeHash(hashWithSaltBytes));
            return resultHashWithSalt;
        }

        public static bool VerifyHash(string hash, string password, byte[] salt)
        {
            var resultHashWithSalt = GenerateSaltedHash(password, salt);
            return hash == resultHashWithSalt;
        }
    }
}
