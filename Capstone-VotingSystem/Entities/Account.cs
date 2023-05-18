using System;
using System.Collections.Generic;

namespace Capstone_VotingSystem.Entities
{
    public partial class Account
    {
        public Guid Id { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public Guid? RoleId { get; set; }

        public virtual Role? Role { get; set; }
    }
}
