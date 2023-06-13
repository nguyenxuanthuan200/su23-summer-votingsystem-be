using System;
using System.Collections.Generic;

namespace Capstone_VotingSystem.Entities
{
    public partial class Candidate
    {
        public Candidate()
        {
            Votings = new HashSet<Voting>();
        }

        public Guid CandidateProfileId { get; set; }
        public string? Description { get; set; }
        public double? Score { get; set; }
        public string? UserId { get; set; }
        public Guid? CampaignId { get; set; }

        public virtual Campaign? Campaign { get; set; }
        public virtual User? User { get; set; }
        public virtual Ratio? Ratio { get; set; }
        public virtual ICollection<Voting> Votings { get; set; }
    }
}
