using System;
using System.Collections.Generic;

namespace Capstone_VotingSystem.Entities
{
    public partial class CampaignDetail
    {
        public Guid CampaignDetailId { get; set; }
        public DateTime? Day { get; set; }
        public Guid? CampaignId { get; set; }
        public int? AmountVote { get; set; }

        public virtual Campaign? Campaign { get; set; }
    }
}
