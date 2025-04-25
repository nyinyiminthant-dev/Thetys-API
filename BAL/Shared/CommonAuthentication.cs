using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace BAL.Shared
{
    public class CommonAuthentication
    {
        public byte[] CreatePasswordHash(string password)
        {
            using (var sha512 = SHA256.Create())
            {
                return sha512.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }


        public bool VerifyPasswordHash(string password, string storedHash)
        {
            using (var sha256 = SHA256.Create())
            {
                var computedHash = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                var computedHashBase64 = Convert.ToBase64String(computedHash);
                return computedHashBase64 == storedHash;
            }
        }

    }
}
