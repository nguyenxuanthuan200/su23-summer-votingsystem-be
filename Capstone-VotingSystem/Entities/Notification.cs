using System;
using System.Collections.Generic;

namespace Capstone_VotingSystem.Entities
{
    public partial class Notification
    {
        public Guid NotificationId { get; set; }
        public string? Titile { get; set; }
        public string? Text { get; set; }
        public string? UserName { get; set; }

        public virtual User? UserNameNavigation { get; set; }
    }
}
