using System;
using System.Collections.Generic;

namespace Capstone_VotingSystem.Entities
{
    public partial class CampaignStage
    {
        public CampaignStage()
        {
            AnswerVotes = new HashSet<AnswerVote>();
            QuestionStages = new HashSet<QuestionStage>();
        }

        public Guid CampaignStageId { get; set; }
        public Guid? CampaignId { get; set; }
        public double? AmountVote { get; set; }

        public virtual Campaign? Campaign { get; set; }
        public virtual ICollection<AnswerVote> AnswerVotes { get; set; }
        public virtual ICollection<QuestionStage> QuestionStages { get; set; }
    }
}
