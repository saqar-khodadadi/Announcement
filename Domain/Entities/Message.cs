
using Domain.Entities.Base;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Message :Entity
    {
        #region CTOR
        private Message() { }
        private Message(string title, string descriptipn, Priority priority)
        {
            Title = title;
            Description = descriptipn;
            Priority = priority;
        }
        #endregion

        #region Properties
        public string Title { get; private set; }
        public string Description { get; private set; }

        public Priority Priority { get; private set; }
        #endregion

        #region NavigationProperties
        public virtual ICollection<Role> Roles { get; set; }
        #endregion

        #region Functionalities
        public static Message New(string title, string descriptipn, Priority priority)
        {
            return new Message(title, descriptipn, priority);
        }

        public void SetRolePermission(HashSet<Role> roles)
        {
            Roles = roles;
        }
        #endregion
    }
}
