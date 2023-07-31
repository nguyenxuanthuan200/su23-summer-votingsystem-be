using System;
using System.Collections.Generic;

namespace Capstone_VotingSystem.Entities
{
    public partial class Account
    {
        public Account()
        {
            Notifications = new HashSet<Notification>();
        }

        public string UserName { get; set; } = null!;
        public string? Password { get; set; }
        public DateTime CreateAt { get; set; }
        public bool Status { get; set; }
        public string? Token { get; set; }
        public Guid RoleId { get; set; }

        public virtual Role Role { get; set; } = null!;
        public virtual User? User { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
    }
}
