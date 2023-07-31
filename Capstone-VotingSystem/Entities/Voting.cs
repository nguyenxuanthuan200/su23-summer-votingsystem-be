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
        public DateTime SendingTime { get; set; }
        public string? Visibility { get; set; }
        public bool Status { get; set; }
        public Guid RatioGroupId { get; set; }
        public string UserId { get; set; } = null!;
        public Guid CandidateId { get; set; }
        public Guid StageId { get; set; }

        public virtual Candidate Candidate { get; set; } = null!;
        public virtual Ratio RatioGroup { get; set; } = null!;
        public virtual Stage Stage { get; set; } = null!;
        public virtual User User { get; set; } = null!;
        public virtual ICollection<VotingDetail> VotingDetails { get; set; }
    }
}
