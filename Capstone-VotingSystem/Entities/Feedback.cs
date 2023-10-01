using System;
using System.Collections.Generic;

namespace Capstone_VotingSystem.Entities
{
    public partial class FeedBack
    {
        public Guid FeedBackId { get; set; }
        public string? Content { get; set; }
        public DateTime CreateDate { get; set; }
        public bool Status { get; set; }
        public string UserId { get; set; } = null!;
        public Guid CampaignId { get; set; }

        public virtual Campaign Campaign { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
