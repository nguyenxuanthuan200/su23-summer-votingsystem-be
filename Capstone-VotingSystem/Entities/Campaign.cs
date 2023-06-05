using System;
using System.Collections.Generic;

namespace Capstone_VotingSystem.Entities
{
    public partial class Campaign
    {
        public Campaign()
        {
            CampaignStages = new HashSet<CampaignStage>();
            CandidateProfiles = new HashSet<CandidateProfile>();
            RatioCategories = new HashSet<RatioCategory>();
        }

        public Guid CampaignId { get; set; }
        public string? Title { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public bool? Visibility { get; set; }
        public bool? Status { get; set; }
        public string? UserName { get; set; }

        public virtual User? UserNameNavigation { get; set; }
        public virtual ICollection<CampaignStage> CampaignStages { get; set; }
        public virtual ICollection<CandidateProfile> CandidateProfiles { get; set; }
        public virtual ICollection<RatioCategory> RatioCategories { get; set; }
    }
}
