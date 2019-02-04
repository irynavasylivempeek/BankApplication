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
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }

        public static string GenerateSaltedHash(string password, byte[] saltBytes)
        {
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] hashBytes = _md5.ComputeHash(passwordBytes);
            byte[] hashWithSaltBytes = hashBytes.Concat(saltBytes).ToArray();
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
