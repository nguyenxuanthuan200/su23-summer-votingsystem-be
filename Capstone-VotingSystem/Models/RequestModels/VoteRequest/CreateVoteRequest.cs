namespace Capstone_VotingSystem.Models.RequestModels.VoteRequest
{
    public class CreateVoteRequest
    {
        public DateTime? Time { get; set; }
        public Guid? VotingId { get; set; }
        public Guid? FormStageId { get; set; }
        public Guid? CandidateProfileId { get; set; }
        public Guid? RatioCategoryId { get; set; }
        public bool? AnswerSelect { get; set; }
    }
}
