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
            GroupUsers = new HashSet<GroupUser>();
            Groups = new HashSet<Group>();
            Ratios = new HashSet<Ratio>();
            Stages = new HashSet<Stage>();
        }

        public Guid CampaignId { get; set; }
        public string Title { get; set; } = null!;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Visibility { get; set; } = null!;
        public string? ImgUrl { get; set; }
        public string? Process { get; set; }
        public bool IsApprove { get; set; }
        public bool Status { get; set; }
        public string UserId { get; set; } = null!;
        public Guid CategoryId { get; set; }

        public virtual Category Category { get; set; } = null!;
        public virtual User User { get; set; } = null!;
        public virtual ICollection<Candidate> Candidates { get; set; }
        public virtual ICollection<FeedBack> FeedBacks { get; set; }
        public virtual ICollection<GroupUser> GroupUsers { get; set; }
        public virtual ICollection<Group> Groups { get; set; }
        public virtual ICollection<Ratio> Ratios { get; set; }
        public virtual ICollection<Stage> Stages { get; set; }
    }
}
