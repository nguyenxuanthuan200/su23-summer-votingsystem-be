using System;
using System.Collections.Generic;

namespace Capstone_VotingSystem.Entities
{
    public partial class Ratio
    {
        public Ratio()
        {
            Votings = new HashSet<Voting>();
        }

        public Guid RatioGroupId { get; set; }
        public double Proportion { get; set; }
        public Guid GroupVoterId { get; set; }
        public Guid CampaignId { get; set; }
        public Guid GroupCandidateId { get; set; }

        public virtual Campaign Campaign { get; set; } = null!;
        public virtual Group GroupCandidate { get; set; } = null!;
        public virtual Group GroupVoter { get; set; } = null!;
        public virtual ICollection<Voting> Votings { get; set; }
    }
}
