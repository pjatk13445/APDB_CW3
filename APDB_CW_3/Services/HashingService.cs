using System;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace APDB_CW_3.Services
{
    public class HashingService
    {
        public static string Hash(string Plain, string Salt)
        {
            var valueBytes = KeyDerivation.Pbkdf2(
                password: Plain,
                salt: Encoding.UTF8.GetBytes(Salt),
                prf: KeyDerivationPrf.HMACSHA512,
                iterationCount: 10000,
                numBytesRequested: 256 / 8);
            return Convert.ToBase64String(valueBytes);
        }

        public static bool Check(string Plain, string Salt, string Hashed)
        {
            return (Hash(Plain, Salt) == Hashed);
        }

        public static string GenerateSalt()
        {
            byte[] randomBytes = new byte[128 / 8];
            RandomNumberGenerator.Create().GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }
    }
}