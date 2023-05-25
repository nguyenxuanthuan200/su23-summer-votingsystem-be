using System;
using System.Collections.Generic;

namespace Capstone_VotingSystem.Entities
{
    public partial class AccountMod
    {
        public AccountMod()
        {
            HistoryMods = new HashSet<HistoryMod>();
        }

        public string Username { get; set; } = null!;
        public string? Password { get; set; }
        public Guid? RoleId { get; set; }
        public string? Token { get; set; }
        public string? Name { get; set; }
        public string? Img { get; set; }
        public bool? Status { get; set; }

        public virtual Role? Role { get; set; }
        public virtual ICollection<HistoryMod> HistoryMods { get; set; }
    }
}
