using System;
using System.Collections.Generic;

namespace Capstone_VotingSystem.Entities
{
    public partial class GroupUser
    {
        public Guid GroupUserId { get; set; }
        public string UserId { get; set; } = null!;
        public Guid GroupId { get; set; }
        public Guid CampaignId { get; set; }

        public virtual Campaign Campaign { get; set; } = null!;
        public virtual Group Group { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
