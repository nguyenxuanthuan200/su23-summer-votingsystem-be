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
        public Guid? GroupId1 { get; set; }
        public Guid? GroupId2 { get; set; }
        public Guid? CampaignId { get; set; }

        public virtual Campaign? Campaign { get; set; }
        public virtual Group? GroupId1Navigation { get; set; }
        public virtual Group? GroupId2Navigation { get; set; }
        public virtual Candidate RatioGroup { get; set; } = null!;
        public virtual ICollection<Voting> Votings { get; set; }
    }
}
