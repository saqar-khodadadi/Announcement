using Domain.Entities.Base;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Role : Entity
    {
        #region CTOR
        private Role() { }
        private Role(string name)
        {
            Name = name;
        }
        #endregion

        #region Properties
        public string Name { get; private set; }
        #endregion

        #region NavigationProperties
        public virtual ICollection<User> Users { get; private set; }
        public virtual ICollection<Message> Messages { get; private set; }
        #endregion

        #region Functionalities
        public static Role New(string name)
        {
            return new Role(name);
        }
        #endregion


    }
}
