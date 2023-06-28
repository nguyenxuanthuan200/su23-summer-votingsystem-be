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
        public decimal? Percent { get; set; }
        public Guid? GroupId { get; set; }
        public Guid? CampaignId { get; set; }
        public Guid? GroupCandidateId { get; set; }

        public virtual Campaign? Campaign { get; set; }
        public virtual Group? Group { get; set; }
        public virtual Candidate? GroupCandidate { get; set; }
        public virtual ICollection<Voting> Votings { get; set; }
    }
}
