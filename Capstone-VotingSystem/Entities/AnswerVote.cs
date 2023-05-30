using System;
using System.Collections.Generic;

namespace Capstone_VotingSystem.Entities
{
    public partial class AnswerVote
    {
        public Guid AnswerVoteId { get; set; }
        public bool? Answer { get; set; }
        public Guid? QuestionStageId { get; set; }
        public Guid? VoteDetailId { get; set; }

        public virtual CampaignStage? QuestionStage { get; set; }
        public virtual VoteDetail? VoteDetail { get; set; }
    }
}
