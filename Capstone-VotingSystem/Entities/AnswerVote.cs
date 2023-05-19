using System;
using System.Collections.Generic;

namespace Capstone_VotingSystem.Entities
{
    public partial class AnswerVote
    {
        public Guid Id { get; set; }
        public bool? Answer { get; set; }
        public Guid? QuestionId { get; set; }
        public Guid? VoteDetailId { get; set; }

        public virtual Question? Question { get; set; }
        public virtual VoteDetail? VoteDetail { get; set; }
    }
}
