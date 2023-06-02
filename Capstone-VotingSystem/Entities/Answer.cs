using System;
using System.Collections.Generic;

namespace Capstone_VotingSystem.Entities
{
    public partial class Answer
    {
        public Guid AnswerId { get; set; }
        public bool? AnswerSelect { get; set; }
        public Guid? VotingDetailId { get; set; }
        public Guid? QuestionId { get; set; }

        public virtual Element AnswerNavigation { get; set; } = null!;
        public virtual Question? Question { get; set; }
        public virtual VotingDetail? VotingDetail { get; set; }
    }
}
