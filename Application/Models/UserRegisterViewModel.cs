using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class UserRegisterViewModel
    {
        #region CTOR
        private UserRegisterViewModel(User user)
        {
            Id = user.Id;
            Username = user.Username;
            FirstName = user.FirstName;
            LastName = user.LastName;
        }
        #endregion

        #region Properties
        public int Id { get; private set; }
        public string Username { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        #endregion

        #region Functionalities
        public static UserRegisterViewModel MakeNew(User user)
        {
            return new UserRegisterViewModel(user);
        }
        #endregion

    }
}
