using System;
using System.Collections.Generic;

namespace Capstone_VotingSystem.Entities
{
    public partial class Notification
    {
        public Guid NotificationId { get; set; }
        public string Title { get; set; } = null!;
        public string Message { get; set; } = null!;
        public DateTime CreateDate { get; set; }
        public bool IsRead { get; set; }
        public bool Status { get; set; }
        public string Username { get; set; } = null!;
        public Guid? CampaignId { get; set; }

        public virtual Campaign? Campaign { get; set; }
        public virtual Account UsernameNavigation { get; set; } = null!;
    }
}
