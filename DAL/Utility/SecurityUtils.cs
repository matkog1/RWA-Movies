using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public static class SecurityUtils
    {
        // Generates a new salt for password hashing
        public static (byte[], string) GenerateSalt()
        {
            byte[] salt = RandomNumberGenerator.GetBytes(128 / 8);
            return (salt, Convert.ToBase64String(salt));
        }

        // Hashes a password with a given salt
        public static string HashPassword(string password, byte[] salt)
        {
            byte[] hash = KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8);

            return Convert.ToBase64String(hash);
        }

        // Generates a new security token
        public static string GenerateSecurityToken()
        {
            byte[] securityToken = RandomNumberGenerator.GetBytes(256 / 8);
            return Convert.ToBase64String(securityToken);
        }

    }
}
