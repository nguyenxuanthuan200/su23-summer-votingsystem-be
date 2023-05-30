using System;
using System.Collections.Generic;

namespace Capstone_VotingSystem.Entities
{
    public partial class Campaign
    {
        public Campaign()
        {
            CampaignStages = new HashSet<CampaignStage>();
            Questions = new HashSet<Question>();
            TeacherCampaigns = new HashSet<TeacherCampaign>();
        }

        public Guid CampaignId { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? Endtime { get; set; }
        public bool? Status { get; set; }
        public Guid? CampusId { get; set; }
        public Guid? CampaignTypeId { get; set; }

        public virtual CampaignType? CampaignType { get; set; }
        public virtual Campus? Campus { get; set; }
        public virtual ICollection<CampaignStage> CampaignStages { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
        public virtual ICollection<TeacherCampaign> TeacherCampaigns { get; set; }
    }
}
