using System;
using System.Collections.Generic;

namespace Capstone_VotingSystem.Entities
{
    public partial class VotingDetail
    {
        public VotingDetail()
        {
            Answers = new HashSet<Answer>();
        }

        public Guid VotingDetailId { get; set; }
        public DateTime? Time { get; set; }
        public Guid? VotingId { get; set; }
        public Guid? FormStageId { get; set; }
        public Guid? CandidateProfileId { get; set; }
        public Guid? RatioCategoryId { get; set; }

        public virtual CandidateProfile? CandidateProfile { get; set; }
        public virtual FormStage? FormStage { get; set; }
        public virtual RatioCategory? RatioCategory { get; set; }
        public virtual Voting? Voting { get; set; }
        public virtual ICollection<Answer> Answers { get; set; }
    }
}
