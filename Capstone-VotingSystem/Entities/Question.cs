using System;
using System.Collections.Generic;

namespace Capstone_VotingSystem.Entities
{
    public partial class Question
    {
        public Question()
        {
            AnswerVotes = new HashSet<AnswerVote>();
        }

        public Guid Id { get; set; }
        public string? Question1 { get; set; }
        public string? Description { get; set; }
        public Guid? CampaignId { get; set; }

        public virtual Campaign? Campaign { get; set; }
        public virtual ICollection<AnswerVote> AnswerVotes { get; set; }
    }
}
