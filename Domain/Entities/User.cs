using Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public record EncryptedPassword  (byte[] passwordSalt, byte[] passwordHash);
    public class User : Entity  , IEquatable<User>
    {
        #region CTOR
        private User() { }

        private User(string firstName, string lastName, string username, byte[] passwordHash, byte[] passwordSalt, Role role)
        {
            FirstName = firstName;
            LastName = lastName;
            Username = username;
            PasswordHash = passwordHash;
            PasswordSalt = passwordSalt;
            Roles = new List<Role>() { role };
        }
        #endregion

        #region Properties
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Username { get; private set; }
        public byte[] PasswordHash { get; private set; }
        public byte[] PasswordSalt { get; private set; }
        #endregion

        #region NavigationProperties

        public virtual ICollection<Role> Roles { get; private set; }
        #endregion

        #region Functionalities

        public static User New(string firstName, string lastName, string username, string password, Role role)
        {
            ValidatePassword(password);
            var encryptedPassword = CreatePasswordHash(password);
            return new User(firstName, lastName,username, encryptedPassword.passwordHash, encryptedPassword.passwordSalt, role);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as User);
        }

        public bool Equals(User other)
        {
            return other != null && PasswordHash == other.PasswordHash;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(PasswordHash);
        }

        public static bool operator == (User user1, User user2)
        {
            return EqualityComparer<User>.Default.Equals(user1, user2);
        }

        public static bool operator !=(User user1, User user2)
        {
            return !(user1 == user2);
        }
        private static void ValidatePassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Password cannot be empty or null.");
            }
            var pattern = "^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[!@#$%^&*()-_+=]).{6,}$";

            if (Regex.IsMatch(password, pattern)) 
            {
                throw new ArgumentException("You are entring wrong format for your password");
            }
        }
        private static EncryptedPassword CreatePasswordHash(string password /*, CancellationToken cancellationToken*/)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            byte[] passwordHash, passwordSalt;
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;

                var byteStream = Encoding.UTF8.GetBytes(password);
                passwordHash = hmac.ComputeHash(byteStream/*, cancellationToken*/);
            }
            return new EncryptedPassword(passwordHash, passwordSalt);
        }
        #endregion

    }
}
