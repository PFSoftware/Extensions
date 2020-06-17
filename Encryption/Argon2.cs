using Konscious.Security.Cryptography;
using PFSoftware.Extensions.DataTypeHelpers;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace PFSoftware.Extensions.Encryption
{
    /// <summary>Provides an extension into the Argon2 hashing scheme.</summary>
    public static class Argon2
    {
        /// <summary>Creates salt for hashing passwords.</summary>
        /// <param name="length">Number of bytes used for length of salt</param>
        /// <returns>Salt for password</returns>
        public static byte[] CreateSalt(int length = 16)
        {
            var buffer = new byte[length];
            var rng = new RNGCryptoServiceProvider();
            rng.GetBytes(buffer);
            return buffer;
        }

        /// <summary>Hashes a password using the Argon2 hashing scheme.</summary>
        /// <param name="password">Plaintext password to be hashed</param>
        /// <param name="salt">Salt to be added to the password</param>
        /// <param name="saltLength">Number of bytes used for length of salt</param>
        /// <param name="parallelism">Degree of parallelism (cores = value / 2)</param>
        /// <param name="iterations">Number of iterations</param>
        /// <param name="memorySize">Memory size in KB</param>
        /// <returns>Hashed password</returns>
        public static string HashPassword(string password, byte[] salt, int saltLength = 16, int parallelism = 8, int iterations = 4, int memorySize = 1024 * 1024)
        {
            Argon2id argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
            {
                Salt = salt,
                DegreeOfParallelism = parallelism,
                Iterations = iterations,
                MemorySize = memorySize
            };

            byte[] hashed = argon2.GetBytes(16);
            string saltString = Convert.ToBase64String(salt);
            string hashedString = Convert.ToBase64String(hashed);
            return $"$argon2$sl={saltLength}$p={parallelism}$i={iterations}$m={memorySize}${saltString}${hashedString}";
        }

        /// <summary>Verifies a password with its hashed counterpart.</summary>
        /// <param name="plaintext">Plaintext password</param>
        /// <param name="hashed">Already hashed password</param>
        public static bool VerifyHash(string plaintext, string hashed)
        {
            string[] pieces = hashed.Split('$');
            if (pieces.Length == 0 || pieces[1] != "argon2") return false;
            int saltLength = Int32Helper.Parse(pieces[2].Substring(3));
            int parallelism = Int32Helper.Parse(pieces[3].Substring(2));
            int iterations = Int32Helper.Parse(pieces[4].Substring(2));
            int memorySize = Int32Helper.Parse(pieces[5].Substring(2));
            string saltString = pieces[6];
            byte[] salt = Convert.FromBase64String(saltString);
            string newHash = HashPassword(plaintext, salt, saltLength, parallelism, iterations, memorySize);
            return hashed.SequenceEqual(newHash);
        }
    }
}