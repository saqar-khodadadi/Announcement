using Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class User : Entity
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

        public static User New(string firstName, string lastName, string username, byte[] passwordHash, byte[] passwordSalt, Role role)
        {
            return new User(firstName, lastName,username, passwordHash, passwordSalt, role);
        }
        #endregion

    }
}
