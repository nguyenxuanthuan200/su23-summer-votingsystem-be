using System;
using System.Collections.Generic;

namespace Capstone_VotingSystem.Entities
{
    public partial class Role
    {
        public Role()
        {
            Accounts = new HashSet<Account>();
        }

        public Guid RoleId { get; set; }
        public string? Name { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }
    }
}
