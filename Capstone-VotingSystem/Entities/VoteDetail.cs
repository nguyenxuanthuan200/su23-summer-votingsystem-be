using System;
using System.Collections.Generic;

namespace Capstone_VotingSystem.Entities
{
    public partial class VoteDetail
    {
        public VoteDetail()
        {
            AnswerVotes = new HashSet<AnswerVote>();
        }

        public Guid Id { get; set; }
        public DateTime? Time { get; set; }
        public Guid? TeacherId { get; set; }
        public string? MssvStudent { get; set; }

        public virtual Student? MssvStudentNavigation { get; set; }
        public virtual Teacher? Teacher { get; set; }
        public virtual ICollection<AnswerVote> AnswerVotes { get; set; }
    }
}
