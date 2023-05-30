using System;
using System.Collections.Generic;

namespace Capstone_VotingSystem.Entities
{
    public partial class QuestionStage
    {
        public Guid QuestionStageId { get; set; }
        public Guid? CampaignStageId { get; set; }
        public Guid? QuestionId { get; set; }

        public virtual CampaignStage? CampaignStage { get; set; }
        public virtual Question? Question { get; set; }
    }
}
