using System;
using System.Security.Cryptography;
using System.Text;

namespace MAMS.API.Tools
{
    public static class Password
    {
        public static string HashPassword(string password)
        {
            using (SHA256 sha = SHA256.Create())
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                byte[] hashedPassword = sha.ComputeHash(passwordBytes);
                return Convert.ToBase64String(hashedPassword);
            }
        }
    }
}
