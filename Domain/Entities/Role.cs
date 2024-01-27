using Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Role : Entity
    {
        public string Name { get; private set; }
        public virtual ICollection<User> Users { get; private set; }
    }
}
