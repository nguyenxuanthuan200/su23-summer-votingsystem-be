using System;
using System.Collections.Generic;

namespace Capstone_VotingSystem.Entities
{
    public partial class Account
    {
        public string UserName { get; set; } = null!;
        public string? Password { get; set; }
        public string? Token { get; set; }
        public bool? Status { get; set; }

        public virtual User? User { get; set; }
    }
}
