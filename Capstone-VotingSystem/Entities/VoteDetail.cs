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

        public Guid VoteDetailId { get; set; }
        public DateTime? Time { get; set; }
        public string? Mssv { get; set; }
        public Guid? TeacherCampaignId { get; set; }

        public virtual Student? MssvNavigation { get; set; }
        public virtual TeacherCampaign? TeacherCampaign { get; set; }
        public virtual ICollection<AnswerVote> AnswerVotes { get; set; }
    }
}
