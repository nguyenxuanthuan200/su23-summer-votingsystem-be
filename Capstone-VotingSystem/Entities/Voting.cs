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

        public Guid VoringId { get; set; }
        public DateTime? SendingTime { get; set; }
        public bool? Status { get; set; }
        public Guid? RatioGroupId { get; set; }
        public string? UserId { get; set; }
        public Guid? CandidateProfileId { get; set; }
        public Guid? StageId { get; set; }

        public virtual Candidate? CandidateProfile { get; set; }
        public virtual Ratio? RatioGroup { get; set; }
        public virtual Stage? Stage { get; set; }
        public virtual User? User { get; set; }
        public virtual ICollection<VotingDetail> VotingDetails { get; set; }
    }
}
