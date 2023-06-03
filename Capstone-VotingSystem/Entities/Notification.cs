using System;
using System.Collections.Generic;

namespace Capstone_VotingSystem.Entities
{
    public partial class Notification
    {
        public Guid NotificationId { get; set; }
        public string? Titile { get; set; }
        public string? Text { get; set; }
        public string? Username { get; set; }

        public virtual User? UsernameNavigation { get; set; }
    }
}
