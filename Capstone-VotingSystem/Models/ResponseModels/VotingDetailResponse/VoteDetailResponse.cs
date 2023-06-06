namespace Capstone_VotingSystem.Models.ResponseModels.VotingDetailResponse
{
    public class VoteDetailResponse
    {
        public Guid VotingDetailId { get; set; }
        public DateTime? Time { get; set; }
        public Guid? VotingId { get; set; }
        public Guid? FormStageId { get; set; }
        public Guid? CandidateProfileId { get; set; }
        public Guid? RatioCategoryId { get; set; }
    }
}
