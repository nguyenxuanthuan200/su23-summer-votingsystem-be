using System;
using System.Collections.Generic;

namespace Capstone_VotingSystem.Entities
{
    public partial class Role
    {
        public Role()
        {
            Accounts = new HashSet<Account>();
            Students = new HashSet<Student>();
        }

        public Guid Id { get; set; }
        public string? Name { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }
        public virtual ICollection<Student> Students { get; set; }
    }
}
