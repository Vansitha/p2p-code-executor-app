using System;
using System.Security.Cryptography;
using System.Text;

namespace DesktopClient
{


    internal class SHA256HashUtility
    {
        // Generate a SHA256 hash for a given string
        public static string GenerateSHA256Hash(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    builder.Append(hashBytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        // Verify if a hash matches a given input string
        public static bool VerifySHA256Hash(string input, string hashToVerify)
        {
            string generatedHash = GenerateSHA256Hash(input);
            return string.Equals(generatedHash, hashToVerify, StringComparison.OrdinalIgnoreCase);
        }
    }
}
