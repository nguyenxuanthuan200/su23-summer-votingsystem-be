﻿namespace Capstone_VotingSystem.Models.RequestModels.VoteDetailRequest
{
    public class CreateVoteDetailRequest
    {
        public DateTime? Time { get; set; }
        public Guid? VotingId { get; set; }
        public Guid? FormStageId { get; set; }
        public Guid? CandidateProfileId { get; set; }
        public Guid? RatioCategoryId { get; set; }
    }
}
