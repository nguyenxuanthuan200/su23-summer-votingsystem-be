using System;
using System.Collections.Generic;

namespace Capstone_VotingSystem.Entities
{
    public partial class Candidate
    {
        public Candidate()
        {
            Scores = new HashSet<Score>();
            Votings = new HashSet<Voting>();
        }

        public Guid CandidateId { get; set; }
        public string? Description { get; set; }
        public bool? Status { get; set; }
        public string? UserId { get; set; }
        public Guid? CampaignId { get; set; }

        public virtual Campaign? Campaign { get; set; }
        public virtual User? User { get; set; }
        public virtual Ratio? Ratio { get; set; }
        public virtual ICollection<Score> Scores { get; set; }
        public virtual ICollection<Voting> Votings { get; set; }
    }
}
