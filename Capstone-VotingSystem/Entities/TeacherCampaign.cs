using System;
using System.Collections.Generic;

namespace Capstone_VotingSystem.Entities
{
    public partial class TeacherCampaign
    {
        public TeacherCampaign()
        {
            VoteDetails = new HashSet<VoteDetail>();
        }

        public Guid TeacherCampaignId { get; set; }
        public double? AmountVote { get; set; }
        public double? Score { get; set; }
        public Guid? CampaignId { get; set; }
        public Guid? TeacherId { get; set; }

        public virtual Campaign? Campaign { get; set; }
        public virtual Teacher? Teacher { get; set; }
        public virtual ICollection<VoteDetail> VoteDetails { get; set; }
    }
}
