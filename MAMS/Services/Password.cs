using System;
using System.Security.Cryptography;
using System.Text;

namespace MAMS.Services
{
    public static class Password
    {
        private const string Salt = "MAMS";

        public static string HashPassword(string password)
        {
            using (SHA256 sha = SHA256.Create())
            {
                // Concatenate the password and the fixed salt
                string saltedPassword = password + Salt;

                // Convert the concatenated string to bytes
                byte[] passwordBytes = Encoding.UTF8.GetBytes(saltedPassword);

                // Compute the hash
                byte[] hashedPassword = sha.ComputeHash(passwordBytes);

                // Convert the hash to a base64 string
                return Convert.ToBase64String(hashedPassword);
            }
        }
    }
}
