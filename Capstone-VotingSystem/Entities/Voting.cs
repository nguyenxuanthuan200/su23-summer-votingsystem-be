using System;
using System.Collections.Generic;

namespace Capstone_VotingSystem.Entities
{
    public partial class Voting
    {
        public Voting()
        {
            VotingDetails = new HashSet<VotingDetail>();
        }

        public Guid VotingId { get; set; }
        public DateTime? Time { get; set; }
        public Guid? CampaignStageId { get; set; }
        public string? UserName { get; set; }

        public virtual CampaignStage? CampaignStage { get; set; }
        public virtual User? UserNameNavigation { get; set; }
        public virtual ICollection<VotingDetail> VotingDetails { get; set; }
    }
}
