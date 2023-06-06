using System;
using System.Collections.Generic;

namespace Capstone_VotingSystem.Entities
{
    public partial class CampaignStage
    {
        public CampaignStage()
        {
            Votings = new HashSet<Voting>();
        }

        public Guid CampaignStageId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public bool? Status { get; set; }
        public string? Text { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public Guid? CampaignId { get; set; }

        public virtual Campaign? Campaign { get; set; }
        public virtual FormStage? FormStage { get; set; }
        public virtual ICollection<Voting> Votings { get; set; }
    }
}
