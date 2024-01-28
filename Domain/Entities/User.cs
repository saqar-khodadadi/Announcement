using Domain.Entities.Base;
using Domain.Entities.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class User : Entity
    {
        #region CTOR
        private User() { }

        private User(string firstName, string lastName, string username, string password, Role role)
        {
            FirstName = firstName;
            LastName = lastName;
            Username = username;
            Password = Password.CreatePasswordHash(password);
            Roles = new List<Role>() { role };
        }
        #endregion

        #region Properties
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Username { get; private set; }
        public Password Password { get; private set; }

        #endregion

        #region NavigationProperties

        public virtual ICollection<Role> Roles { get; private set; }
        #endregion

        #region Functionalities

        public static User New(string firstName, string lastName, string username, string password, Role role)
        {
            return new User(firstName, lastName,username, password, role);
        }
        #endregion

    }
}
