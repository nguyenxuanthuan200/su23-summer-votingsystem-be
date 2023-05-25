using System;
using System.Collections.Generic;

namespace Capstone_VotingSystem.Entities
{
    public partial class Role
    {
        public Role()
        {
            AccountMods = new HashSet<AccountMod>();
        }

        public Guid RoleId { get; set; }
        public string? Name { get; set; }

        public virtual ICollection<AccountMod> AccountMods { get; set; }
    }
}
