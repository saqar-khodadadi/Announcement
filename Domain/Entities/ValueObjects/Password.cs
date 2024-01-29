using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Domain.Entities.ValueObjects
{
    public class Password : IEquatable<Password>
    {
        private readonly byte[] _passwordHash;
        private readonly byte[] _passwordSalt;

        private Password(byte[] passwordHash, byte[] passwordSalt)
        {
            _passwordHash = passwordHash;
            _passwordSalt = passwordSalt;
        }

        public byte[] PasswordHash => _passwordHash;
        public byte[] PasswordSalt => _passwordSalt;

        public static Password CreatePasswordHash(string password)
        {
            ValidatePassword(password);
            byte[] passwordHash, passwordSalt;
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;

                var byteStream = Encoding.UTF8.GetBytes(password);
                passwordHash = hmac.ComputeHash(byteStream/*, cancellationToken*/);
            }
            if (!VerifyPasswordHash(password, passwordHash, passwordSalt)) throw new Exception("hashed password is wrong");
            return new Password(passwordHash, passwordSalt);
        }
        private static void ValidatePassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Password cannot be empty or null.");
            }
            var pattern = "^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[!@#$%^&*()-_+=]).{6,}$";

            if (!Regex.IsMatch(password, pattern))
            {
                throw new ArgumentException("You are entring wrong format for your password");
            }
        }
        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");
            if (storedSalt.Length != 256)

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }
        public override bool Equals(object obj)
        {
            return Equals(obj as Password);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(PasswordHash);
        }

        public static bool operator ==(Password password1, Password password2)
        {
            return EqualityComparer<Password>.Default.Equals(password1, password2);
        }

        public static bool operator !=(Password password1, Password password2)
        {
            return !(password1 == password2);
        }

        public bool Equals(Password other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return PasswordHash == other.PasswordHash && PasswordSalt == other.PasswordSalt;
        }
    }

}
