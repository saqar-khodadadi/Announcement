using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Outputs
{
    public class UserLoginResultDto
    {

        #region CTOR
        private UserLoginResultDto(int id, string username, string firstName, string lastName, string token)
        {
            Id = id;
            Username = username;
            FirstName = firstName;
            LastName = lastName;
            Token = token;
        }
        #endregion

        #region Properties
        public int Id { get; private set; }
        public string Username { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Token { get; private set; }
        #endregion


        #region Functionalities
        public static UserLoginResultDto MakeNew(int id, string username, string firstName, string lastName, string token)
        {
            return new UserLoginResultDto(id, username, firstName, lastName, token);
        }
        #endregion
    }
}
