using System;
using System.Collections.Generic;

namespace Capstone_VotingSystem.Entities
{
    public partial class Candidate
    {
        public Candidate()
        {
            ActivityContents = new HashSet<ActivityContent>();
            Ratios = new HashSet<Ratio>();
            Scores = new HashSet<Score>();
            Votings = new HashSet<Voting>();
        }

        public Guid CandidateId { get; set; }
        public string? Description { get; set; }
        public string? FullName { get; set; }
        public string? AvatarUrl { get; set; }
        public bool? Status { get; set; }
        public string? UserId { get; set; }
        public Guid? CampaignId { get; set; }

        public virtual Campaign? Campaign { get; set; }
        public virtual User? User { get; set; }
        public virtual ICollection<ActivityContent> ActivityContents { get; set; }
        public virtual ICollection<Ratio> Ratios { get; set; }
        public virtual ICollection<Score> Scores { get; set; }
        public virtual ICollection<Voting> Votings { get; set; }
    }
}
