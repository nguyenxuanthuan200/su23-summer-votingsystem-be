using System;
using System.Collections.Generic;

namespace Capstone_VotingSystem.Entities
{
    public partial class Campaign
    {
        public Campaign()
        {
            Candidates = new HashSet<Candidate>();
            FeedBacks = new HashSet<FeedBack>();
            Ratios = new HashSet<Ratio>();
            Stages = new HashSet<Stage>();
        }

        public Guid CampaignId { get; set; }
        public string? Title { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string? Visibility { get; set; }
        public string? ImgUrl { get; set; }
        public bool? Status { get; set; }
        public Guid? UserId { get; set; }
        public Guid? CategoryId { get; set; }

        public virtual Category? Category { get; set; }
        public virtual ICollection<Candidate> Candidates { get; set; }
        public virtual ICollection<FeedBack> FeedBacks { get; set; }
        public virtual ICollection<Ratio> Ratios { get; set; }
        public virtual ICollection<Stage> Stages { get; set; }
    }
}
